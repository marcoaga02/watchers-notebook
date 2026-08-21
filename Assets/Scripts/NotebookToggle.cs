using UnityEngine;

public class NotebookToggle : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;
    [SerializeField] private GameObject notebookPanel;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            notebookPanel.SetActive(!notebookPanel.activeSelf);
        }
    }
}
