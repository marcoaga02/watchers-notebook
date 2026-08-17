using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Watcher/Behavior")]
public class Behavior : ScriptableObject
{
    public LocalizedString displayName;
    public Sprite icon;
    public CapabilitySigil satisfies;
}
