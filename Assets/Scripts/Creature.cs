using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private CreatureDefinition definition;

    public bool CanUse(CapabilitySigil capability)
    {
        return definition != null && definition.Implements(capability);
    }
}
