using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class CreatureJournalRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text implementsLabel;
    [SerializeField] private LocalizedString nameLabelFormat;
    [SerializeField] private LocalizedString implementsLabelFormat;

    public void Setup(CreatureDefinition species, IReadOnlyList<CapabilitySigil> collectedSigils)
    {
        icon.sprite = species.Icon;
        nameLabel.text = $"{nameLabelFormat.GetLocalizedString()} {species.DisplayName.GetLocalizedString()}";

        var implemented = collectedSigils.Where(species.Implements).ToList();
        var implementedText = implemented.Count == 0
            ? "???"
            : string.Join(", ", implemented.Select(sigil => sigil.InterfaceName));

        implementsLabel.text = $"{implementsLabelFormat.GetLocalizedString()} {implementedText}";
    }
}
