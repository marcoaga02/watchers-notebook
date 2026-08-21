using UnityEngine;
using UnityEngine.Localization;

[RequireComponent(typeof(CreatureMover))]
public class ObstaclePrompt : MonoBehaviour
{
    [SerializeField] private KeyCode openKey = KeyCode.E;
    [SerializeField] private LocalizedString promptText;
    [SerializeField] private GameObject evocationPanel;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private PossessionController possessionController;

    private CreatureMover _mover;
    private string _resolvedPromptText;
    private bool _isShowingPrompt;

    private void Awake()
    {
        _mover = GetComponent<CreatureMover>();
        _resolvedPromptText = promptText.GetLocalizedString();
    }

    private void Update()
    {
        if (evocationPanel.activeSelf || !panelManager.CanOpen(evocationPanel))
        {
            HidePrompt();
            return;
        }

        if (possessionController.IsPossessing)
        {
            HidePrompt();

            if (Input.GetKeyDown(openKey))
            {
                evocationPanel.SetActive(true);
            }

            return;
        }

        var aheadPosition = transform.position + (Vector3)_mover.Facing;
        var required = TerrainProbe.Instance.GetRequiredCapability(aheadPosition);

        if (required == null)
        {
            HidePrompt();
            return;
        }

        ShowPrompt();

        if (Input.GetKeyDown(openKey))
        {
            evocationPanel.SetActive(true);
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (_isShowingPrompt)
        {
            return;
        }

        _isShowingPrompt = true;
        WorldPrompt.Instance.Show(_resolvedPromptText, transform);
    }

    private void HidePrompt()
    {
        if (!_isShowingPrompt)
        {
            return;
        }

        _isShowingPrompt = false;
        WorldPrompt.Instance.Hide();
    }
}
