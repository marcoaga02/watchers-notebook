using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Creature))]
public class CreatureMover : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");

    [SerializeField] private float speed = 4f;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Creature creature;
    [Tooltip("On: the rig has real left/right art (MoveX carries sign, no flip). " +
             "Off: only one side exists, mirrored via flipX (Orc, old Player).")]
    [SerializeField] private bool hasDirectionalSprites = true;

    private Rigidbody2D _rb;
    private bool _hasAnimator;

    private Vector2 _input;

    public Vector2 Facing { get; private set; } = Vector2.down;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hasAnimator = _animator != null;
    }

    public void SetInput(Vector2 input)
    {
        _input = input.normalized;
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
        var velocity = _input * speed;
        var nextPosition = _rb.position + velocity * Time.fixedDeltaTime;

        var required = TerrainProbe.Instance.GetRequiredCapability(nextPosition);
        if (required != null && !creature.CanUse(required))
        {
            velocity = Vector2.zero;
        }

        _rb.linearVelocity = velocity;
    }

    private void UpdateAnimation()
    {
        if (!_hasAnimator)
        {
            return;
        }

        var isMoving = _input.sqrMagnitude > 0.01f;
        _animator.SetBool(IsMoving, isMoving);

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
