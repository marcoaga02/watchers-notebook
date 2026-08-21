using UnityEngine;

public class NotebookToggle : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private GameObject notebookPanel;
    [SerializeField] private PanelManager panelManager;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!notebookPanel.activeSelf && !panelManager.CanOpen(notebookPanel))
            {
                return;
            }

            notebookPanel.SetActive(!notebookPanel.activeSelf);
        }
    }
}
