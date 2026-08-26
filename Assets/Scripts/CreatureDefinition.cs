using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Watcher/Creature Definition")]
public class CreatureDefinition : ScriptableObject
{
    public string className;
    public CreatureDefinition parent;
    public List<CapabilitySigil> declaredCapabilities = new();

    [Header("Evocation")]
    public LocalizedString displayName;
    public Sprite icon;
    public GameObject prefab;

    public bool Implements(CapabilitySigil capability)
    {
        if (capability == null)
        {
            return true;
        }

        var current = this;
        while (current != null)
        {
            if (current.declaredCapabilities.Contains(capability))
            {
                return true;
            }

            current = current.parent;
        }

        return false;
    }

    private IEnumerable<CapabilitySigil> AllCapabilities()
    {
        var all = new HashSet<CapabilitySigil>();
        var current = this;
        while (current != null)
        {
            all.UnionWith(current.declaredCapabilities);
            current = current.parent;
        }

        return all;
    }

    public bool MatchesExactly(IEnumerable<CapabilitySigil> capabilities)
    {
        var all = new HashSet<CapabilitySigil>(AllCapabilities());
        return all.SetEquals(capabilities);
    }
}