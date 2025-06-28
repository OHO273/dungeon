using System.Collections.Generic;
using UnityEngine;

public class HexCorridorGenerator
{
    public void GenerateCorridors(List<List<Vector2Int>> corridors, HexMapData data)
    {
        foreach (var path in corridors)
        {
            foreach (var pos in path)
            {
                var tile = data.GetTile(pos.x, pos.y);
                if (tile != null && tile.tileType == TileType.None)
                {
                    tile.SetAsCorridor();
                }
            }
        }
    }
}