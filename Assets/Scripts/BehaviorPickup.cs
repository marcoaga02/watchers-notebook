using UnityEngine;
using UnityEngine.Localization;

public class BehaviorPickup : Pickup
{
    [SerializeField] private Behavior behavior;
    [SerializeField] private LocalizedString promptFormat;

    protected override Sprite Icon => behavior != null ? behavior.icon : null;
    protected override string PromptText => behavior != null ? promptFormat.GetLocalizedString(behavior.displayName.GetLocalizedString()) : string.Empty;

    protected override void Collect()
    {
        PlayerInventory.Instance.Collect(behavior);
    }
}
