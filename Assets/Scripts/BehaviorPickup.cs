using UnityEngine;

public class BehaviorPickup : Pickup
{
    [SerializeField] private Behavior behavior;

    protected override Sprite Icon => behavior != null ? behavior.icon : null;
    protected override string ItemName => behavior != null ? behavior.displayName.GetLocalizedString() : string.Empty;

    protected override void Collect()
    {
        PlayerInventory.Instance.Collect(behavior);
    }
}
