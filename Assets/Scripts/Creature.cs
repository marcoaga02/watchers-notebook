using System;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Serializable]
    private struct TestBinding
    {
        public CapabilitySigil capability;
        public Behavior behavior;
    }

    [SerializeField] private CreatureDefinition definition;

    // TODO: remove this field and the Awake() logic that consumes it once the real
    // evocation panel exists; Bind() will be called only at runtime from there.
    [Header("Test binding (remove once the panel exists)")]
    [SerializeField] private TestBinding[] testBindings;

    private readonly CapabilityBinding _binding = new();

    private void Awake()
    {
        foreach (var entry in testBindings)
        {
            _binding.Bind(entry.capability, entry.behavior);
        }
    }

    public void Bind(CapabilitySigil capability, Behavior behavior)
    {
        _binding.Bind(capability, behavior);
    }

    public bool CanUse(CapabilitySigil capability)
    {
        return definition != null && definition.Implements(capability) && _binding.Satisfies(capability);
    }
}
