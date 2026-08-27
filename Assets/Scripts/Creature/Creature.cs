using System;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    [Serializable]
    private struct InitialBinding
    {
        [SerializeField] private CapabilitySigil capability;
        [SerializeField] private Behavior behavior;

        public CapabilitySigil Capability => capability;
        public Behavior Behavior => behavior;
    }

    [SerializeField] private CreatureDefinition definition;
    [Tooltip("Bindings applied on spawn, for creatures that are never evoked through PossessionController (e.g. wild creatures with CreatureAI).")]
    [SerializeField] private List<InitialBinding> initialBindings = new();

    private readonly CapabilityBinding _binding = new();

    private void Awake()
    {
        foreach (var binding in initialBindings)
        {
            Bind(binding.Capability, binding.Behavior);
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

    public Behavior GetBehavior(CapabilitySigil capability)
    {
        return _binding.GetBehavior(capability);
    }
}
