using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;
    public Vector2 offset;

    private Vector3 _velocity;

    private void LateUpdate()
    {
        // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
        if (target == null)
        {
            return;
        }

        var goal = new Vector3(target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref _velocity, smoothTime);
    }
}