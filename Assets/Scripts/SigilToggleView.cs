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

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    public void Setup(CapabilitySigil sigil)
    {
        Sigil = sigil;
        label.text = sigil.displayName.GetLocalizedString();
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool _)
    {
        ValueChanged?.Invoke();
    }
}
