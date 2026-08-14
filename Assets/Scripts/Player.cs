using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    
    [SerializeField] private float speed = 4f;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _hasAnimator;
    
    private Vector2 _input;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hasAnimator = TryGetComponent(out _animator);
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")).normalized;
        
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _input * speed;
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