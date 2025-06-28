using UnityEngine;

public class HexMapVisualizer : MonoBehaviour
{
    public void Visualize(HexTile[,] tiles, float hexRadius, GameObject floorPrefab, GameObject[] wallPrefabs, Transform parent)
    {
        int width = tiles.GetLength(0);
        int height = tiles.GetLength(1);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = tiles[x, y];
                GameObject toSpawn = null;
                if (tile.isFloor || tile.isCorridor)
                {
                    toSpawn = floorPrefab;
                }
                else if (tile.isWall)
                {
                    toSpawn = wallPrefabs[(int)tile.wallType];
                }
                if (toSpawn == null) continue;
                var go = Instantiate(toSpawn, tile.worldPosition, Quaternion.identity, parent);
                go.name = $"Tile_{x}_{y}_{tile.tileType}";
            }
        }
    }
}