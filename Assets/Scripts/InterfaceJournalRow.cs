using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceJournalRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text implementingCreaturesLabel;
    [SerializeField] private TMP_Text implementationsLabel;

    public void Setup(CapabilitySigil sigil, IReadOnlyList<CreatureDefinition> observedSpecies, IReadOnlyList<Behavior> collectedBehaviors)
    {
        icon.sprite = sigil.icon;
        nameLabel.text = sigil.interfaceName;

        var implementingSpecies = observedSpecies.Where(species => species.Implements(sigil)).ToList();
        implementingCreaturesLabel.text = implementingSpecies.Count == 0
            ? "???"
            : string.Join(", ", implementingSpecies.Select(species => species.displayName.GetLocalizedString()));

        var implementations = collectedBehaviors.Where(behavior => behavior.Satisfies(sigil)).ToList();
        implementationsLabel.text = implementations.Count == 0
            ? "???"
            : string.Join(", ", implementations.Select(behavior => behavior.displayName.GetLocalizedString()));
    }
}
