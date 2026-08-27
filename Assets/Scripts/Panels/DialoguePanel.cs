using System;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public static DialoguePanel Instance { get; private set; }

    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text label;
    [SerializeField] private KeyCode advanceKey = KeyCode.E;

    public GameObject PanelRoot => panelRoot;

    private Action _onClosed;

    private void Awake()
    {
        Instance = this;
        panelRoot.SetActive(false);
    }

    public void Show(string text, Action onClosed)
    {
        label.text = text;
        _onClosed = onClosed;
        panelRoot.SetActive(true);
    }

    private void Update()
    {
        if (panelRoot.activeSelf && Input.GetKeyDown(advanceKey))
        {
            panelRoot.SetActive(false);
            var callback = _onClosed;
            _onClosed = null;
            callback?.Invoke();
        }
    }
}
