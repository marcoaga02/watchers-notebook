using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Pickup : MonoBehaviour
{
    [SerializeField] private KeyCode collectKey = KeyCode.E;

    private bool _playerInRange;

    protected abstract Sprite Icon { get; }
    protected abstract string PromptText { get; }
    protected abstract bool AlreadyCollected { get; }
    protected abstract void Collect();

    private void Awake()
    {
        // the zone scene reloads from scratch on every visit, so a pickup destroyed on a
        // previous visit would otherwise reappear even though it's already in the inventory
        if (AlreadyCollected)
        {
            Destroy(gameObject);
            return;
        }

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
        WorldPrompt.Instance.Show(PromptText, other.transform);
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
