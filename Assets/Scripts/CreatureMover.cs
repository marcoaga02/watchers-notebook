using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Creature))]
public class CreatureMover : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    [SerializeField] private float speed = 4f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Creature creature;
    [Tooltip("On: the rig has real left/right art (MoveX carries sign, no flip). " +
             "Off: only one side exists, mirrored via flipX (Orc, old Player).")]
    [SerializeField] private bool hasDirectionalSprites = true;

    private Rigidbody2D _rb;
    private bool _hasAnimator;
    private bool _hasRunningParameter;

    private Vector2 _input;
    private bool _isSprinting;

    public Vector2 Facing { get; private set; } = Vector2.down;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hasAnimator = _animator != null;
        // not every rig has a running state (only Man does), so this parameter is optional
        _hasRunningParameter = _hasAnimator && System.Array.Exists(_animator.parameters, p => p.nameHash == IsRunning);
    }

    public void SetInput(Vector2 input)
    {
        _input = input.normalized;
    }

    public void SetSprinting(bool value)
    {
        _isSprinting = value;
    }

    public void SetFacing(Vector2 direction)
    {
        if (direction.sqrMagnitude <= 0.01f)
        {
            return;
        }

        Facing = Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
            ? new Vector2(Mathf.Sign(direction.x), 0f)
            : new Vector2(0f, Mathf.Sign(direction.y));

        if (!_hasAnimator)
        {
            return;
        }

        _animator.SetFloat(MoveX, Facing.x);
        _animator.SetFloat(MoveY, Facing.y);
        if (!hasDirectionalSprites)
        {
            _spriteRenderer.flipX = Facing.x < 0f;
        }
    }

    public void SetControlEnabled(bool value)
    {
        enabled = value;
        _rb.linearVelocity = Vector2.zero;
        if (_spriteRenderer != null)
        {
            _spriteRenderer.enabled = value;
        }
    }

    private void Update()
    {
        UpdateFacing();
        UpdateAnimation();
    }

    private void UpdateFacing()
    {
        if (_input.sqrMagnitude <= 0.01f)
        {
            return;
        }

        Facing = Mathf.Abs(_input.x) > 0.01f
            ? new Vector2(Mathf.Sign(_input.x), 0f)
            : new Vector2(0f, Mathf.Sign(_input.y));
    }

    private void FixedUpdate()
    {
        var velocity = _input * speed * CurrentSpeedMultiplier() * (_isSprinting ? sprintMultiplier : 1f);

        if (TerrainProbe.Instance != null)
        {
            var nextPosition = _rb.position + velocity * Time.fixedDeltaTime;
            var currentCell = TerrainProbe.Instance.GetCell(_rb.position);
            var nextCell = TerrainProbe.Instance.GetCell(nextPosition);

            if (nextCell != currentCell)
            {
                var required = TerrainProbe.Instance.GetRequiredCapability(nextPosition);
                if (required != null && !creature.CanUse(required))
                {
                    velocity = Vector2.zero;
                }
            }
        }

        if (_rb.bodyType == RigidbodyType2D.Kinematic)
        {
            _rb.MovePosition(_rb.position + velocity * Time.fixedDeltaTime);
        }
        else
        {
            _rb.linearVelocity = velocity;
        }
    }

    private float CurrentSpeedMultiplier()
    {
        if (TerrainProbe.Instance == null)
        {
            return 1f;
        }

        var currentCapability = TerrainProbe.Instance.GetRequiredCapability(_rb.position);
        if (currentCapability == null)
        {
            return 1f;
        }

        var behavior = creature.GetBehavior(currentCapability);
        return behavior != null ? behavior.speedMultiplier : 1f;
    }

    public bool IsStuck()
    {
        if (TerrainProbe.Instance == null)
        {
            return false;
        }

        var currentRequired = TerrainProbe.Instance.GetRequiredCapability(_rb.position);
        if (currentRequired != null && !creature.CanUse(currentRequired))
        {
            return true;
        }

        if (CurrentSpeedMultiplier() <= 0f)
        {
            return true;
        }

        var aheadPosition = _rb.position + Facing;
        var aheadRequired = TerrainProbe.Instance.GetRequiredCapability(aheadPosition);
        return aheadRequired != null && !creature.CanUse(aheadRequired);
    }

    private void UpdateAnimation()
    {
        if (!_hasAnimator)
        {
            return;
        }

        var isMoving = _input.sqrMagnitude > 0.01f;
        _animator.SetBool(IsMoving, isMoving);
        if (_hasRunningParameter)
        {
            _animator.SetBool(IsRunning, isMoving && _isSprinting);
        }

        if (!isMoving)
        {
            return;
        }

        if (Mathf.Abs(_input.x) > 0.01f) // there is horizontal movement
        {
            _animator.SetFloat(MoveX, hasDirectionalSprites ? Mathf.Sign(_input.x) : 1f);
            _animator.SetFloat(MoveY, 0f);
            if (!hasDirectionalSprites)
            {
                _spriteRenderer.flipX = _input.x < 0f;
            }
        }
        else // there is vertical movement
        {
            _animator.SetFloat(MoveX, 0f);
            _animator.SetFloat(MoveY, Mathf.Sign(_input.y));
        }
    }
}
