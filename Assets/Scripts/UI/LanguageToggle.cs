using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageToggle : MonoBehaviour
{
    [Tooltip("Optional: rebuilt on toggle so its rows pick up the new locale immediately, " +
             "since its entries resolve their localized text once, on enable.")]
    [SerializeField] private GameObject notebookPanel;

    public void ToggleLanguage()
    {
        var locales = LocalizationSettings.AvailableLocales.Locales;
        var current = LocalizationSettings.SelectedLocale;
        var nextIndex = (locales.IndexOf(current) + 1) % locales.Count;
        LocalizationSettings.SelectedLocale = locales[nextIndex];

        if (notebookPanel != null && notebookPanel.activeSelf)
        {
            notebookPanel.SetActive(false);
            notebookPanel.SetActive(true);
        }
    }
}
