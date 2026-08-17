using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BehaviorBindingRow : MonoBehaviour
{
    [SerializeField] private TMP_Text capabilityLabel;
    [SerializeField] private TMP_Dropdown dropdown;

    public CapabilitySigil Capability { get; private set; }
    public Behavior SelectedBehavior { get; private set; }

    public event Action SelectionChanged;

    private List<Behavior> _options;

    public void Setup(CapabilitySigil capability, IReadOnlyList<Behavior> availableBehaviors)
    {
        Capability = capability;
        capabilityLabel.text = capability.displayName.GetLocalizedString();

        _options = new List<Behavior>(availableBehaviors);
        dropdown.ClearOptions();
        dropdown.AddOptions(_options.ConvertAll(behavior => behavior.displayName.GetLocalizedString()));
        dropdown.onValueChanged.AddListener(OnDropdownChanged);

        OnDropdownChanged(dropdown.value);
    }

    private void OnDropdownChanged(int index)
    {
        SelectedBehavior = index >= 0 && index < _options.Count ? _options[index] : null;
        SelectionChanged?.Invoke();
    }
}
