using UnityEngine;

[RequireComponent(typeof(CreatureMover))]
public class ObstaclePrompt : MonoBehaviour
{
    [SerializeField] private KeyCode openKey = KeyCode.E;
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private GameObject evocationPanel;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private PossessionController possessionController;
    [SerializeField] private Vector3 worldOffset = new(0f, 0.6f, 0f);

    private CreatureMover _mover;
    private RectTransform _promptRect;
    private Camera _camera;

    private void Awake()
    {
        _mover = GetComponent<CreatureMover>();
        _promptRect = promptRoot.GetComponent<RectTransform>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (evocationPanel.activeSelf || possessionController.IsPossessing)
        {
            promptRoot.SetActive(false);
            return;
        }

        var aheadPosition = transform.position + (Vector3)_mover.Facing;
        var required = TerrainProbe.Instance.GetRequiredCapability(aheadPosition);
        var showPrompt = required != null;

        promptRoot.SetActive(showPrompt);

        if (showPrompt)
        {
            FollowPlayer();

            if (Input.GetKeyDown(openKey))
            {
                evocationPanel.SetActive(true);
                promptRoot.SetActive(false);
            }
        }
    }

    private void FollowPlayer()
    {
        var screenPoint = _camera.WorldToScreenPoint(transform.position + worldOffset);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out var localPoint);
        _promptRect.anchoredPosition = localPoint;
    }
}
