using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ScreenFollower : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Vector3 worldOffset = new(0f, 0.6f, 0f);

    private RectTransform _rect;
    private Camera _camera;
    private Transform _target;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _camera = Camera.main;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        var screenPoint = _camera.WorldToScreenPoint(_target.position + worldOffset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out var localPoint);
        _rect.anchoredPosition = localPoint;
    }
}
