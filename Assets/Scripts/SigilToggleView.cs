using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SigilToggleView : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private Toggle _toggle;

    public CapabilitySigil Sigil { get; private set; }
    public bool IsOn => _toggle.isOn;

    public event Action ValueChanged;

    public void Setup(CapabilitySigil sigil)
    {
        _toggle = GetComponent<Toggle>();
        Sigil = sigil;
        label.text = sigil.InterfaceName;
        label.ForceMeshUpdate();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool _)
    {
        ValueChanged?.Invoke();
    }
}
