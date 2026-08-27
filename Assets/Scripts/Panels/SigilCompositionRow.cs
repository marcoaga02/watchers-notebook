using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SigilCompositionRow : MonoBehaviour
{
    [SerializeField] private SigilToggleView toggleView;
    [SerializeField] private BehaviorBindingRow bindingRow;
    [SerializeField] private GameObject noImplementationsLabel;

    public CapabilitySigil Sigil => toggleView.Sigil;
    public bool IsOn => toggleView.IsOn;
    public Behavior SelectedBehavior => bindingRow.SelectedBehavior;

    public event Action ValueChanged;

    private bool _hasImplementations;

    public void Setup(CapabilitySigil sigil, IReadOnlyList<Behavior> availableBehaviors)
    {
        toggleView.Setup(sigil);
        toggleView.ValueChanged += HandleChanged;

        bindingRow.Setup(sigil, availableBehaviors);
        bindingRow.SelectionChanged += HandleChanged;

        _hasImplementations = availableBehaviors.Any(behavior => behavior.Satisfies(sigil));

        SetBindingVisible(false);
    }

    public void SetBindingVisible(bool visible)
    {
        bindingRow.gameObject.SetActive(visible && _hasImplementations);
        noImplementationsLabel.SetActive(visible && !_hasImplementations);
    }

    private void OnDestroy()
    {
        toggleView.ValueChanged -= HandleChanged;
        bindingRow.SelectionChanged -= HandleChanged;
    }

    private void HandleChanged()
    {
        ValueChanged?.Invoke();
    }
}
