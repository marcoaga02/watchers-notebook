using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    public Transform target;
    public float smoothTime = 0.15f;
    public Vector2 offset;

    private Vector3 _velocity;

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position, Goal(), ref _velocity, smoothTime);
    }

    public void Snap()
    {
        if (target == null)
        {
            return;
        }

        transform.position = Goal();
        _velocity = Vector3.zero;
    }

    private Vector3 Goal()
    {
        return new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
    }
}