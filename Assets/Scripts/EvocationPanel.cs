using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvocationPanel : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PossessionController possessionController;

    [Header("Sigil composition")]
    [SerializeField] private Transform sigilRowContainer;
    [SerializeField] private SigilCompositionRow sigilRowPrefab;
    [SerializeField] private TMP_Text noInterfaceLabel;

    [Header("Matched creature")]
    [SerializeField] private CreatureRow creatureRow;
    [SerializeField] private TMP_Text noCreatureLabel;

    [Header("Evocation")]
    [SerializeField] private Button evokeButton;

    private CreatureDefinition _matched;
    private readonly List<SigilCompositionRow> _sigilRows = new();

    private void OnEnable()
    {
        foreach (var sigil in inventory.CollectedSigils)
        {
            var row = Instantiate(sigilRowPrefab, sigilRowContainer);
            row.Setup(sigil, inventory.CollectedBehaviors);
            row.ValueChanged += Refresh;
            _sigilRows.Add(row);
        }

        noInterfaceLabel.gameObject.SetActive(_sigilRows.Count == 0);

        Refresh();
    }

    private void OnDisable()
    {
        foreach (var row in _sigilRows)
        {
            row.ValueChanged -= Refresh;
            Destroy(row.gameObject);
        }

        _sigilRows.Clear();
    }

    private void Refresh()
    {
        var selected = _sigilRows.Where(row => row.IsOn).Select(row => row.Sigil).ToList();

        _matched = inventory.ObservedSpecies.FirstOrDefault(species => species.MatchesExactly(selected));

        foreach (var row in _sigilRows)
        {
            row.SetBindingVisible(row.IsOn);
        }

        if (_matched == null)
        {
            creatureRow.gameObject.SetActive(false);
            noCreatureLabel.gameObject.SetActive(true);
            evokeButton.interactable = false;
            return;
        }

        creatureRow.gameObject.SetActive(true);
        creatureRow.Setup(_matched);
        noCreatureLabel.gameObject.SetActive(false);

        UpdateEvokeButton();
    }

    private void UpdateEvokeButton()
    {
        var activeRows = _sigilRows.Where(row => row.IsOn);
        evokeButton.interactable = _matched != null && activeRows.All(row => row.SelectedBehavior != null);
    }

    public void Evoke()
    {
        if (_matched == null || !evokeButton.interactable)
        {
            return;
        }

        var bindings = _sigilRows.Where(row => row.IsOn)
            .Select(row => new KeyValuePair<CapabilitySigil, Behavior>(row.Sigil, row.SelectedBehavior));

        possessionController.Evoke(_matched.prefab, bindings);
        gameObject.SetActive(false);
    }
}
