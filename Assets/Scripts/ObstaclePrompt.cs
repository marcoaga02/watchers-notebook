using UnityEngine;
using UnityEngine.Localization;

[RequireComponent(typeof(CreatureMover))]
public class ObstaclePrompt : MonoBehaviour
{
    [SerializeField] private KeyCode openKey = KeyCode.E;
    [SerializeField] private LocalizedString promptText;
    [SerializeField] private LocalizedString stuckPromptText;
    [SerializeField] private GameObject evocationPanel;
    [SerializeField] private PossessionController possessionController;

    private CreatureMover _mover;
    private string _resolvedPromptText;
    private string _resolvedStuckPromptText;
    private bool _isShowingPrompt;

    private void Awake()
    {
        _mover = GetComponent<CreatureMover>();
        _resolvedPromptText = promptText.GetLocalizedString();
        _resolvedStuckPromptText = stuckPromptText.GetLocalizedString();
    }

    private void Update()
    {
        if (evocationPanel.activeSelf || !PanelManager.Instance.CanOpen(evocationPanel))
        {
            HidePrompt();
            return;
        }

        if (possessionController.IsPossessing)
        {
            UpdatePossessed();
            return;
        }

        if (TerrainProbe.Instance == null)
        {
            HidePrompt();
            return;
        }

        var aheadPosition = (Vector3)(_mover.GroundPosition + _mover.Facing);
        var required = TerrainProbe.Instance.GetRequiredCapability(aheadPosition);

        if (required == null)
        {
            HidePrompt();
            return;
        }

        ShowPrompt(_resolvedPromptText, transform);

        if (Input.GetKeyDown(openKey))
        {
            evocationPanel.SetActive(true);
            HidePrompt();
        }
    }

    private void UpdatePossessed()
    {
        var possessed = possessionController.Possessed;
        if (possessed == null || !possessed.IsStuck())
        {
            HidePrompt();
            return;
        }

        ShowPrompt(_resolvedStuckPromptText, possessed.transform);

        if (Input.GetKeyDown(openKey))
        {
            evocationPanel.SetActive(true);
            HidePrompt();
        }
    }

    private void ShowPrompt(string text, Transform target)
    {
        _isShowingPrompt = true;
        WorldPrompt.Instance.Show(text, target);
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
