using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Watcher/Capability Sigil")]
public class CapabilitySigil : ScriptableObject
{
    public string interfaceName;
    public string methodSignature;
    public LocalizedString displayName;
    public Sprite icon;
}
