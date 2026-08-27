using TMPro;
using UnityEngine;

public class WorldPrompt : MonoBehaviour
{
    public static WorldPrompt Instance { get; private set; }

    [SerializeField] private GameObject promptRoot;
    [SerializeField] private TMP_Text label;
    [SerializeField] private ScreenFollower screenFollower;

    private void Awake()
    {
        Instance = this;
        WarmUpFont();
        promptRoot.SetActive(false);
    }

    // Forces TextMeshPro to generate glyphs for the characters we use while
    // the scene is still loading, so the very first real Show() does not
    // stall on font atlas generation.
    private void WarmUpFont()
    {
        promptRoot.SetActive(true);
        label.text = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789():.,àèìòù";
        label.ForceMeshUpdate();
    }

    public void Show(string text, Transform target)
    {
        label.text = text;
        screenFollower.SetTarget(target);
        promptRoot.SetActive(true);
    }

    public void Hide()
    {
        screenFollower.SetTarget(null);
        promptRoot.SetActive(false);
    }
}
