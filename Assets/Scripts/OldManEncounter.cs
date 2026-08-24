using UnityEngine;
using UnityEngine.Localization;

public class OldManEncounter : MonoBehaviour
{
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Vector2 facingDirection = Vector2.down;
    [SerializeField] private float sightRange = 4f;
    [SerializeField] private LayerMask sightMask;
    [SerializeField] private CreatureDefinition creatureToReveal;
    [SerializeField] private LocalizedString dialogueLine;
    [SerializeField] private bool showDebugRay;

    private bool _hasTriggered;

    private void Start()
    {
        GetComponent<CreatureMover>().SetFacing(facingDirection);
    }

    private void Update()
    {
        if (_hasTriggered || PanelManager.Instance.IsAnyPanelOpen)
        {
            return;
        }

        var origin = rayOrigin.position;
        var hit = Physics2D.Raycast(origin, facingDirection, sightRange, sightMask);

        if (showDebugRay)
        {
            Debug.DrawRay(origin, facingDirection.normalized * sightRange, hit.collider != null ? Color.green : Color.red);
        }

        if (hit.collider == null || !hit.collider.TryGetComponent<PlayerInput>(out var playerInput))
        {
            return;
        }

        _hasTriggered = true;
        playerInput.GetComponent<CreatureMover>().SetFacing(-facingDirection);
        DialoguePanel.Instance.Show(dialogueLine.GetLocalizedString(), OnDialogueClosed);
    }

    private void OnDialogueClosed()
    {
        PlayerInventory.Instance.Observe(creatureToReveal);
    }
}
