using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Watcher/Capability Sigil")]
public class CapabilitySigil : ScriptableObject
{
    [SerializeField] private string interfaceName;
    [SerializeField] private string methodSignature;
    [SerializeField] private LocalizedString displayName;
    [SerializeField] private Sprite icon;

    public string InterfaceName => interfaceName;
    public string MethodSignature => methodSignature;
    public Sprite Icon => icon;
}
