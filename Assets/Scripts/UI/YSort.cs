using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class YSort : MonoBehaviour
{
    [SerializeField] private int precision = 100;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        _renderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * precision);
    }
}
