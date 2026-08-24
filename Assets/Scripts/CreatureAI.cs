using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
public class CreatureAI : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float playerBlockDistance = 1f;
    [SerializeField] private float playerBlockWidth = 0.6f;
    [SerializeField] private LayerMask playerMask;

    private CreatureMover _mover;
    private Collider2D _collider;
    private Transform _target;
    private Vector2 _travelDirection;

    private void Awake()
    {
        _mover = GetComponent<CreatureMover>();
        _collider = GetComponent<Collider2D>();
        _target = pointB;
        _travelDirection = ((Vector2)pointB.position - (Vector2)pointA.position).normalized;
    }

    private void Update()
    {
        Vector2 toTarget = _target.position - transform.position;
        if (Vector2.Dot(toTarget, _travelDirection) <= 0f)
        {
            _target = _target == pointA ? pointB : pointA;
            _travelDirection = -_travelDirection;
            toTarget = _target.position - transform.position;
        }

        if (IsPlayerAhead(toTarget))
        {
            _mover.SetInput(Vector2.zero);
            return;
        }

        _mover.SetInput(toTarget);
    }

    private bool IsPlayerAhead(Vector2 direction)
    {
        var hit = Physics2D.CircleCast(_collider.bounds.center, playerBlockWidth, direction, playerBlockDistance, playerMask);
        return hit.collider != null;
    }
}
