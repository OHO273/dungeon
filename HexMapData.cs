using UnityEngine;

public class HexMapData
{
    public int width;
    public int height;
    public float hexRadius;
    public int seed;
    public HexTile[,] tiles;

    public HexMapData(int width, int height, float hexRadius, int seed)
    {
        this.width = width;
        this.height = height;
        this.hexRadius = hexRadius;
        this.seed = seed;

        tiles = new HexTile[width, height];
        float tileWidth = hexRadius * 2f;
        float tileHeight = Mathf.Sqrt(3f) / 2f * tileWidth;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float posX = x * (tileWidth * 0.75f);
                float posY = y * tileHeight + (x % 2 == 1 ? tileHeight / 2f : 0f);
                // �N�a�ϥ����אּ XY�A�H��K 2D ���
                Vector3 worldPos = new Vector3(posX, posY, 0f);
                tiles[x, y] = new HexTile(new Vector2Int(x, y), worldPos);
            }
        }
    }

    public HexTile GetTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;
        return tiles[x, y];
    }
}