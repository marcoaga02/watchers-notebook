using System;
using System.Collections.Generic;
using UnityEngine;

public class SigilCompositionRow : MonoBehaviour
{
    [SerializeField] private SigilToggleView toggleView;
    [SerializeField] private BehaviorBindingRow bindingRow;

    public CapabilitySigil Sigil => toggleView.Sigil;
    public bool IsOn => toggleView.IsOn;
    public Behavior SelectedBehavior => bindingRow.SelectedBehavior;

    public event Action ValueChanged;

    public void Setup(CapabilitySigil sigil, IReadOnlyList<Behavior> availableBehaviors)
    {
        toggleView.Setup(sigil);
        toggleView.ValueChanged += HandleChanged;

        bindingRow.Setup(sigil, availableBehaviors);
        bindingRow.SelectionChanged += HandleChanged;

        bindingRow.gameObject.SetActive(false);
    }

    public void SetBindingVisible(bool visible)
    {
        bindingRow.gameObject.SetActive(visible);
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
