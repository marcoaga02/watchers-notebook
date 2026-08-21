using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SigilPickup : MonoBehaviour
{
    [SerializeField] private CapabilitySigil sigil;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sigil.icon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerInput>(out _))
        {
            return;
        }

        PlayerInventory.Instance.Collect(sigil);
        Destroy(gameObject);
    }
}
