using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureJournalRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameLabel;
    [SerializeField] private TMP_Text implementsLabel;

    public void Setup(CreatureDefinition species, IReadOnlyList<CapabilitySigil> collectedSigils)
    {
        icon.sprite = species.icon;
        nameLabel.text = species.displayName.GetLocalizedString();

        var implemented = collectedSigils.Where(species.Implements).ToList();

        implementsLabel.text = implemented.Count == 0
            ? "???"
            : string.Join(", ", implemented.Select(sigil => sigil.interfaceName));
    }
}
