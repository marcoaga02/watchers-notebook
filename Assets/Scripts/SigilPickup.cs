using UnityEngine;

public class SigilPickup : Pickup
{
    [SerializeField] private CapabilitySigil sigil;

    protected override Sprite Icon => sigil != null ? sigil.icon : null;
    protected override string ItemName => sigil != null ? sigil.interfaceName : string.Empty;

    protected override void Collect()
    {
        PlayerInventory.Instance.Collect(sigil);
    }
}
