using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [SerializeField] private List<CapabilitySigil> collectedSigils = new();
    [SerializeField] private List<Behavior> collectedBehaviors = new();
    [SerializeField] private List<CreatureDefinition> observedSpecies = new();
    [SerializeField] private List<string> metNpcs = new();

    public IReadOnlyList<CapabilitySigil> CollectedSigils => collectedSigils;
    public IReadOnlyList<Behavior> CollectedBehaviors => collectedBehaviors;
    public IReadOnlyList<CreatureDefinition> ObservedSpecies => observedSpecies;

    private void Awake()
    {
        Instance = this;
    }

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

    public bool HasMetNpc(string npcId)
    {
        return metNpcs.Contains(npcId);
    }

    public void MarkNpcMet(string npcId)
    {
        if (!metNpcs.Contains(npcId))
        {
            metNpcs.Add(npcId);
        }
    }
}
