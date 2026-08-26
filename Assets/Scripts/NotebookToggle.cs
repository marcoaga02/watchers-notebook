using UnityEngine;

public class NotebookToggle : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private GameObject notebookPanel;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (!notebookPanel.activeSelf && !PanelManager.Instance.CanOpen(notebookPanel))
            {
                return;
            }

            notebookPanel.SetActive(!notebookPanel.activeSelf);
        }
    }
}
