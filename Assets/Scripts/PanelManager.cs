using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels = new();

    public bool CanOpen(GameObject panel) => panels.Where(p => p != panel).All(p => !p.activeSelf);

    public bool IsAnyPanelOpen => panels.Any(p => p.activeSelf);
}
