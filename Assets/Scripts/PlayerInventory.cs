using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // TODO: remove these serialized lists once collecting Sigils/Behaviors
    // in the world exists; for now they are filled by hand in the Inspector.
    [SerializeField] private List<CapabilitySigil> collectedSigils = new();
    [SerializeField] private List<Behavior> collectedBehaviors = new();
    [SerializeField] private List<CreatureDefinition> observedSpecies = new();

    public IReadOnlyList<CapabilitySigil> CollectedSigils => collectedSigils;
    public IReadOnlyList<Behavior> CollectedBehaviors => collectedBehaviors;
    public IReadOnlyList<CreatureDefinition> ObservedSpecies => observedSpecies;

    public void Collect(CapabilitySigil sigil)
    {
        if (!collectedSigils.Contains(sigil))
        {
            collectedSigils.Add(sigil);
        }
    }

    public void Collect(Behavior behavior)
    {
        if (!collectedBehaviors.Contains(behavior))
        {
            collectedBehaviors.Add(behavior);
        }
    }

    public void Observe(CreatureDefinition species)
    {
        if (!observedSpecies.Contains(species))
        {
            observedSpecies.Add(species);
        }
    }
}
