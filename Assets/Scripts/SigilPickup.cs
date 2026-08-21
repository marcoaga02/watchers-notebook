using UnityEngine;
using UnityEngine.Localization;

public class SigilPickup : Pickup
{
    [SerializeField] private CapabilitySigil sigil;
    [SerializeField] private LocalizedString promptFormat;

    protected override Sprite Icon => sigil != null ? sigil.icon : null;
    protected override string PromptText => sigil != null ? promptFormat.GetLocalizedString(sigil.interfaceName) : string.Empty;

    protected override void Collect()
    {
        PlayerInventory.Instance.Collect(sigil);
    }
}
