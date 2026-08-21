using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text label;

    public void Setup(CreatureDefinition creature)
    {
        icon.sprite = creature.icon;
        label.text = creature.displayName.GetLocalizedString();
        label.ForceMeshUpdate();
    }
}
