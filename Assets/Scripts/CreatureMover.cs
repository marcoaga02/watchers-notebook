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

    private Rigidbody2D _rb;
    private bool _hasAnimator;

    private Vector2 _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hasAnimator = _animator != null;
    }

    public void SetInput(Vector2 input)
    {
        _input = input.normalized;
    }

    private void Update()
    {
        UpdateAnimation();
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
            _animator.SetFloat(MoveX, 1f);
            _animator.SetFloat(MoveY, 0f);
            _spriteRenderer.flipX = _input.x < 0f;
        }
        else // there is vertical movement
        {
            _animator.SetFloat(MoveX, 0f);
            _animator.SetFloat(MoveY, Mathf.Sign(_input.y));
        }
    }
}
