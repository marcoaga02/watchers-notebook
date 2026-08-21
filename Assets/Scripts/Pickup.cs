using UnityEngine;
using UnityEngine.Localization;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private KeyCode collectKey = KeyCode.E;
    [SerializeField] private LocalizedString promptFormat;

    private bool _playerInRange;

    protected abstract Sprite Icon { get; }
    protected abstract string ItemName { get; }
    protected abstract void Collect();

    private void Awake()
    {
        UpdateSprite();
    }

    private void OnValidate()
    {
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (Icon != null)
        {
            GetComponent<SpriteRenderer>().sprite = Icon;
        }
    }

    private void Update()
    {
        if (_playerInRange && Input.GetKeyDown(collectKey))
        {
            Collect();
            WorldPrompt.Instance.Hide();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerInput>(out _))
        {
            return;
        }

        _playerInRange = true;
        WorldPrompt.Instance.Show(promptFormat.GetLocalizedString(ItemName), other.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<PlayerInput>(out _))
        {
            return;
        }

        _playerInRange = false;
        WorldPrompt.Instance.Hide();
    }
}
