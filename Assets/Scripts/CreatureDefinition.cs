using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Watcher/Creature Definition")]
public class CreatureDefinition : ScriptableObject
{
    public string className;
    public CreatureDefinition parent;
    public List<CapabilitySigil> declaredCapabilities = new();

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
}