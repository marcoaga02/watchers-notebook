using UnityEngine;

[CreateAssetMenu(menuName = "Watcher/Behavior")]
public class Behavior : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public CapabilitySigil satisfies;
}
