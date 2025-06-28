using System.Collections.Generic;
using UnityEngine;

public class HexRoom
{
    public int id;
    public List<HexTile> tiles;

    public HexRoom(int id)
    {
        this.id = id;
        tiles = new List<HexTile>();
    }

    public Vector2Int Center
    {
        get
        {
            int sumX = 0, sumY = 0;
            foreach (var t in tiles)
            {
                sumX += t.gridPosition.x;
                sumY += t.gridPosition.y;
            }
            return new Vector2Int(sumX / tiles.Count, sumY / tiles.Count);
        }
    }
}