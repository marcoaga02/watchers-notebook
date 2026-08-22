using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainProbe : MonoBehaviour
{
    public static TerrainProbe Instance { get; private set; }

    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap waterMap;
    [SerializeField] private Tilemap chasmMap;
    [SerializeField] private CapabilitySigil swimmingSigil;
    [SerializeField] private CapabilitySigil flyingSigil;

    public CapabilitySigil SwimmingSigil => swimmingSigil;
    public CapabilitySigil FlyingSigil => flyingSigil;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3Int GetCell(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

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