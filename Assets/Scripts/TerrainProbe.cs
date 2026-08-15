using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainProbe : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap waterMap;
    [SerializeField] private Tilemap chasmMap;

    public string GetRequiredCapability(Vector3 worldPos)
    {
        var cell = grid.WorldToCell(worldPos);
        if (waterMap.HasTile(cell)) return "ISwimming";
        if (chasmMap.HasTile(cell)) return "IFlying";
        return null;
    }

    private void Update()
    {
        var capability = GetRequiredCapability(transform.position);
        Debug.Log(capability ?? "libero");
    }
}