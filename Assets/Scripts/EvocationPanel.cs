using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class EvocationPanel : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private List<CreatureDefinition> knownSpecies = new();
    [SerializeField] private PossessionController possessionController;

    [Header("Sigil selection")]
    [SerializeField] private Transform sigilToggleContainer;
    [SerializeField] private SigilToggleView sigilTogglePrefab;

    [Header("Matched creature")]
    [SerializeField] private TMP_Text matchedSpeciesLabel;
    [SerializeField] private LocalizedString matchedSpeciesPrefix;

    [Header("Behavior binding")]
    [SerializeField] private Transform bindingRowContainer;
    [SerializeField] private BehaviorBindingRow bindingRowPrefab;

    [Header("Evocation")]
    [SerializeField] private Button evokeButton;

    private CreatureDefinition _matched;
    private readonly List<BehaviorBindingRow> _rows = new();
    private readonly List<SigilToggleView> _sigilToggles = new();

    private void OnEnable()
    {
        foreach (var sigil in inventory.CollectedSigils)
        {
            var toggleView = Instantiate(sigilTogglePrefab, sigilToggleContainer);
            toggleView.Setup(sigil);
            toggleView.ValueChanged += Refresh;
            _sigilToggles.Add(toggleView);
        }

        Refresh();
    }

    private void OnDisable()
    {
        foreach (var toggleView in _sigilToggles)
        {
            toggleView.ValueChanged -= Refresh;
            Destroy(toggleView.gameObject);
        }

        _sigilToggles.Clear();
        ClearRows();
    }

    private void Refresh()
    {
        var selected = _sigilToggles.Where(toggleView => toggleView.IsOn).Select(toggleView => toggleView.Sigil).ToList();

        _matched = knownSpecies.FirstOrDefault(species => species.MatchesExactly(selected));

        ClearRows();

        if (_matched == null)
        {
            matchedSpeciesLabel.gameObject.SetActive(false);
            evokeButton.interactable = false;
            return;
        }

        matchedSpeciesLabel.gameObject.SetActive(true);
        matchedSpeciesLabel.text = $"{matchedSpeciesPrefix.GetLocalizedString()} {_matched.displayName.GetLocalizedString()}";

        foreach (var capability in selected)
        {
            var row = Instantiate(bindingRowPrefab, bindingRowContainer);
            row.Setup(capability, inventory.CollectedBehaviors);
            row.SelectionChanged += UpdateEvokeButton;
            _rows.Add(row);
        }

        UpdateEvokeButton();
    }

    private void UpdateEvokeButton()
    {
        evokeButton.interactable = _matched != null && _rows.All(row => row.SelectedBehavior != null);
    }

    private void ClearRows()
    {
        foreach (var row in _rows)
        {
            row.SelectionChanged -= UpdateEvokeButton;
            Destroy(row.gameObject);
        }

        _rows.Clear();
    }

    public void Evoke()
    {
        if (_matched == null || !evokeButton.interactable)
        {
            return;
        }

        var bindings = _rows.Select(row =>
            new KeyValuePair<CapabilitySigil, Behavior>(row.Capability, row.SelectedBehavior));

        possessionController.Evoke(_matched.prefab, bindings);
        gameObject.SetActive(false);
    }
}
