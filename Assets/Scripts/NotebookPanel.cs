using System.Collections.Generic;
using UnityEngine;

public class NotebookPanel : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;

    [Header("Creatures")]
    [SerializeField] private Transform creatureEntriesContainer;
    [SerializeField] private CreatureJournalRow creatureEntryPrefab;

    [Header("Interfaces")]
    [SerializeField] private Transform interfaceEntriesContainer;
    [SerializeField] private InterfaceJournalRow interfaceEntryPrefab;

    private readonly List<CreatureJournalRow> _creatureEntries = new();
    private readonly List<InterfaceJournalRow> _interfaceEntries = new();

    private void OnEnable()
    {
        foreach (var species in inventory.ObservedSpecies)
        {
            var entry = Instantiate(creatureEntryPrefab, creatureEntriesContainer);
            entry.Setup(species, inventory.CollectedSigils);
            _creatureEntries.Add(entry);
        }

        foreach (var sigil in inventory.CollectedSigils)
        {
            var entry = Instantiate(interfaceEntryPrefab, interfaceEntriesContainer);
            entry.Setup(sigil, inventory.ObservedSpecies, inventory.CollectedBehaviors);
            _interfaceEntries.Add(entry);
        }
    }

    private void OnDisable()
    {
        foreach (var entry in _creatureEntries)
        {
            Destroy(entry.gameObject);
        }

        _creatureEntries.Clear();

        foreach (var entry in _interfaceEntries)
        {
            Destroy(entry.gameObject);
        }

        _interfaceEntries.Clear();
    }
}
