using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainProbe : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap waterMap;
    [SerializeField] private Tilemap chasmMap;
    [SerializeField] private CapabilitySigil swimmingSigil;
    [SerializeField] private CapabilitySigil flyingSigil;

    public CapabilitySigil GetRequiredCapability(Vector3 worldPos)
    {
        var cell = grid.WorldToCell(worldPos);
        if (waterMap.HasTile(cell))
        {
            return swimmingSigil;
        }

        if (chasmMap.HasTile(cell))
        {
            return flyingSigil;
        }

        return null;
    }
}