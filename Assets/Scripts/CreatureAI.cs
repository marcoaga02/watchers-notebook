using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
public class CreatureAI : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private CreatureMover _mover;
    private Transform _target;
    private Vector2 _travelDirection;

    private void Awake()
    {
        _mover = GetComponent<CreatureMover>();
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

        _mover.SetInput(toTarget);
    }
}
