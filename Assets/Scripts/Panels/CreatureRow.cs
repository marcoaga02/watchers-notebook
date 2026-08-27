using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text label;

    public void Setup(CreatureDefinition creature)
    {
        icon.sprite = creature.Icon;
        label.text = creature.DisplayName.GetLocalizedString();
        label.ForceMeshUpdate();
    }
}
