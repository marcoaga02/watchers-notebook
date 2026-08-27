using UnityEngine;
using UnityEngine.Localization;

public class OldManEncounter : MonoBehaviour
{
    [Tooltip("Unique per instance, used to remember across zone reloads whether this NPC already forced a stop.")]
    [SerializeField] private string npcId;
    [SerializeField] private KeyCode talkKey = KeyCode.E;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Vector2 facingDirection = Vector2.down;
    [SerializeField] private float sightRange = 4f;
    [SerializeField] private LayerMask sightMask;
    [SerializeField] private CreatureDefinition creatureToReveal;
    [SerializeField] private LocalizedString dialogueLine;
    [SerializeField] private LocalizedString talkPromptText;
    [SerializeField] private bool showDebugRay;

    private bool _hasTriggered;
    private bool _isShowingPrompt;

    private void Start()
    {
        GetComponent<CreatureMover>().SetFacing(facingDirection);
        // the zone scene reloads from scratch on every visit, so without this the forced
        // stop would trigger again every time the player re-enters the zone
        _hasTriggered = PlayerInventory.Instance.HasMetNpc(npcId);
    }

    private void Update()
    {
        if (PanelManager.Instance.IsAnyPanelOpen)
        {
            HidePrompt();
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
            HidePrompt();
            return;
        }

        if (!_hasTriggered)
        {
            _hasTriggered = true;
            PlayerInventory.Instance.MarkNpcMet(npcId);
            playerInput.GetComponent<CreatureMover>().SetFacing(-facingDirection);
            DialoguePanel.Instance.Show(dialogueLine.GetLocalizedString(), OnDialogueClosed);
            return;
        }

        _isShowingPrompt = true;
        WorldPrompt.Instance.Show(talkPromptText.GetLocalizedString(), transform);

        if (Input.GetKeyDown(talkKey))
        {
            HidePrompt();
            DialoguePanel.Instance.Show(dialogueLine.GetLocalizedString(), OnDialogueClosed);
        }
    }

    private void HidePrompt()
    {
        if (!_isShowingPrompt)
        {
            return;
        }

        _isShowingPrompt = false;
        WorldPrompt.Instance.Hide();
    }

    private void OnDialogueClosed()
    {
        if (creatureToReveal != null)
        {
            PlayerInventory.Instance.Observe(creatureToReveal);
        }
    }
}
