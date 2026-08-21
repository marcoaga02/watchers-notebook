using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BehaviorPickup : MonoBehaviour
{
    [SerializeField] private Behavior behavior;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = behavior.icon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerInput>(out _))
        {
            return;
        }

        PlayerInventory.Instance.Collect(behavior);
        Destroy(gameObject);
    }
}
