using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    [SerializeField] private List<GameObject> panels = new();

    private void Awake()
    {
        Instance = this;
    }

    public bool CanOpen(GameObject panel) => panels.Where(p => p != panel).All(p => !p.activeSelf);

    public bool IsAnyPanelOpen => panels.Any(p => p.activeSelf);
}
