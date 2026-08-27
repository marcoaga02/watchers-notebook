using UnityEngine;

public class Bobbing : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float speed = 2f;

    private Vector3 _basePosition;

    private void Awake()
    {
        _basePosition = transform.localPosition;
    }

    private void Update()
    {
        var offset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = _basePosition + new Vector3(0f, offset, 0f);
    }
}
