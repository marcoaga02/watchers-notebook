using System.Collections.Generic;
using UnityEngine;

public class NotebookPanel : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;

    [Header("Creatures")]
    [SerializeField] private Transform creatureEntriesContainer;
    [SerializeField] private CreatureJournalRow creatureEntryPrefab;
    [SerializeField] private GameObject noCreaturesLabel;

    [Header("Interfaces")]
    [SerializeField] private Transform interfaceEntriesContainer;
    [SerializeField] private InterfaceJournalRow interfaceEntryPrefab;
    [SerializeField] private GameObject noInterfacesLabel;

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

        noCreaturesLabel.SetActive(_creatureEntries.Count == 0);

        foreach (var sigil in inventory.CollectedSigils)
        {
            var entry = Instantiate(interfaceEntryPrefab, interfaceEntriesContainer);
            entry.Setup(sigil, inventory.ObservedSpecies, inventory.CollectedBehaviors);
            _interfaceEntries.Add(entry);
        }

        noInterfacesLabel.SetActive(_interfaceEntries.Count == 0);
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
