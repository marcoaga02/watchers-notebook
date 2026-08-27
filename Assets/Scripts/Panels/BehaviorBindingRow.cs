using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class BehaviorBindingRow : MonoBehaviour
{
    [SerializeField] private TMP_Text capabilityLabel;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private LocalizedString implementationOfFormat;

    public CapabilitySigil Capability { get; private set; }
    public Behavior SelectedBehavior { get; private set; }

    public event Action SelectionChanged;

    private List<Behavior> _options;

    public void Setup(CapabilitySigil capability, IReadOnlyList<Behavior> availableBehaviors)
    {
        Capability = capability;
        capabilityLabel.text = implementationOfFormat.GetLocalizedString(capability.MethodSignature);
        capabilityLabel.ForceMeshUpdate();

        _options = availableBehaviors.Where(behavior => behavior.Satisfies(capability)).ToList();
        dropdown.ClearOptions();
        dropdown.AddOptions(_options.ConvertAll(behavior => behavior.DisplayName.GetLocalizedString()));
        dropdown.onValueChanged.AddListener(OnDropdownChanged);

        OnDropdownChanged(dropdown.value);
    }

    private void OnDropdownChanged(int index)
    {
        SelectedBehavior = index >= 0 && index < _options.Count ? _options[index] : null;
        SelectionChanged?.Invoke();
    }
}
