using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class InterfaceJournalRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text implementingCreaturesLabel;
    [SerializeField] private TMP_Text implementationsLabel;
    [SerializeField] private LocalizedString nameLabelFormat;
    [SerializeField] private LocalizedString implementersLabelFormat;
    [SerializeField] private LocalizedString implementationsLabelFormat;

    public void Setup(CapabilitySigil sigil, IReadOnlyList<CreatureDefinition> observedSpecies, IReadOnlyList<Behavior> collectedBehaviors)
    {
        icon.sprite = sigil.Icon;
        nameLabel.text = $"{nameLabelFormat.GetLocalizedString()} {sigil.InterfaceName}";

        var implementingSpecies = observedSpecies.Where(species => species.Implements(sigil)).ToList();
        var implementingText = implementingSpecies.Count == 0
            ? "???"
            : string.Join(", ", implementingSpecies.Select(species => species.DisplayName.GetLocalizedString()));
        implementingCreaturesLabel.text = $"{implementersLabelFormat.GetLocalizedString()} {implementingText}";

        var implementations = collectedBehaviors.Where(behavior => behavior.Satisfies(sigil)).ToList();
        var implementationsText = implementations.Count == 0
            ? "???"
            : string.Join(", ", implementations.Select(behavior => behavior.DisplayName.GetLocalizedString()));
        implementationsLabel.text = $"{implementationsLabelFormat.GetLocalizedString()} {implementationsText}";
    }
}
