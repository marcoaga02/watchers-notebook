using UnityEngine;

public class Creature : MonoBehaviour
{
    [SerializeField] private CreatureDefinition definition;

    private readonly CapabilityBinding _binding = new();

    public void Bind(CapabilitySigil capability, Behavior behavior)
    {
        _binding.Bind(capability, behavior);
    }

    public bool CanUse(CapabilitySigil capability)
    {
        return definition != null && definition.Implements(capability) && _binding.Satisfies(capability);
    }
}
