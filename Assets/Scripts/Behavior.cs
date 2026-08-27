using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Watcher/Behavior")]
public class Behavior : ScriptableObject
{
    [SerializeField] private LocalizedString displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private CapabilitySigil satisfies;
    [SerializeField] private float speedMultiplier = 1f;

    public LocalizedString DisplayName => displayName;
    public Sprite Icon => icon;
    public float SpeedMultiplier => speedMultiplier;

    public bool Satisfies(CapabilitySigil capability) => satisfies == capability;
}
