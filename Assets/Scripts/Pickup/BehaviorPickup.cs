using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

public class BehaviorPickup : Pickup
{
    [SerializeField] private Behavior behavior;
    [SerializeField] private LocalizedString promptFormat;

    protected override Sprite Icon => behavior != null ? behavior.Icon : null;
    protected override string PromptText => behavior != null ? promptFormat.GetLocalizedString(behavior.DisplayName.GetLocalizedString()) : string.Empty;
    protected override bool AlreadyCollected => behavior != null && PlayerInventory.Instance.CollectedBehaviors.Contains(behavior);

    protected override void Collect()
    {
        PlayerInventory.Instance.Collect(behavior);
    }
}
