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
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Creature creature;
    [Tooltip("On: the rig has real left/right art (MoveX carries sign, no flip). " +
             "Off: only one side exists, mirrored via flipX (Orc, old Player).")]
    [SerializeField] private bool hasDirectionalSprites = true;
    [Tooltip("Offset from the Rigidbody2D position to the visual feet, used for every terrain " +
             "check. Match it to the Box Collider 2D's own offset so the gate lines up with " +
             "what's actually drawn on screen, not the sprite's pivot.")]
    [SerializeField] private Vector2 groundCheckOffset;
    [Tooltip("Draws the grid cell the movement gate currently evaluates for this body, " +
             "to compare it against the visible tile boundary.")]
    [SerializeField] private bool showDebugGateCell;

    private Rigidbody2D _rb;
    private bool _hasAnimator;
    private bool _hasRunningParameter;

    private Vector2 _input;
    private bool _isSprinting;
    private Vector3? _debugCellCenter;

    public Vector2 Facing { get; private set; } = Vector2.down;

    public Vector2 GroundPosition => _rb.position + groundCheckOffset;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hasAnimator = animator != null;
        // not every rig has a running state (only Man does), so this parameter is optional
        _hasRunningParameter = _hasAnimator && System.Array.Exists(animator.parameters, p => p.nameHash == IsRunning);
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

        animator.SetFloat(MoveX, Facing.x);
        animator.SetFloat(MoveY, Facing.y);
        if (!hasDirectionalSprites)
        {
            spriteRenderer.flipX = Facing.x < 0f;
        }
    }

    public void SetControlEnabled(bool value)
    {
        enabled = value;
        _rb.linearVelocity = Vector2.zero;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = value;
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
            var nextGroundPosition = GroundPosition + velocity * Time.fixedDeltaTime;
            var currentCell = TerrainProbe.Instance.GetCell(GroundPosition);
            var nextCell = TerrainProbe.Instance.GetCell(nextGroundPosition);

            if (nextCell != currentCell)
            {
                var required = TerrainProbe.Instance.GetRequiredCapability(nextGroundPosition);
                if (required != null && !creature.CanUse(required))
                {
                    velocity = Vector2.zero;
                }
            }

            if (showDebugGateCell)
            {
                _debugCellCenter = TerrainProbe.Instance.CellCenter(currentCell);
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

        var currentCapability = TerrainProbe.Instance.GetRequiredCapability(GroundPosition);
        if (currentCapability == null)
        {
            return 1f;
        }

        var behavior = creature.GetBehavior(currentCapability);
        return behavior != null ? behavior.SpeedMultiplier : 1f;
    }

    public bool IsStuck()
    {
        if (TerrainProbe.Instance == null)
        {
            return false;
        }

        var currentRequired = TerrainProbe.Instance.GetRequiredCapability(GroundPosition);
        if (currentRequired != null && !creature.CanUse(currentRequired))
        {
            return true;
        }

        if (CurrentSpeedMultiplier() <= 0f)
        {
            return true;
        }

        var aheadPosition = GroundPosition + Facing;
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
        animator.SetBool(IsMoving, isMoving);
        if (_hasRunningParameter)
        {
            animator.SetBool(IsRunning, isMoving && _isSprinting);
        }

        if (!isMoving)
        {
            return;
        }

        if (Mathf.Abs(_input.x) > 0.01f) // there is horizontal movement
        {
            animator.SetFloat(MoveX, hasDirectionalSprites ? Mathf.Sign(_input.x) : 1f);
            animator.SetFloat(MoveY, 0f);
            if (!hasDirectionalSprites)
            {
                spriteRenderer.flipX = _input.x < 0f;
            }
        }
        else // there is vertical movement
        {
            animator.SetFloat(MoveX, 0f);
            animator.SetFloat(MoveY, Mathf.Sign(_input.y));
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugGateCell || _debugCellCenter == null)
        {
            return;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_debugCellCenter.Value, Vector3.one);
    }
}
