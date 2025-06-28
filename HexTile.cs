using UnityEngine;

public enum TileType { None, Floor, Corridor, Wall }
public enum WallType { Full, Vertical, Diagonal1, Diagonal2 }

public class HexTile
{
    public Vector2Int gridPosition;
    public Vector3 worldPosition;
    public TileType tileType = TileType.None;
    public bool isRoom = false;
    public bool isCorridor = false;
    public int roomID = -1;
    public WallType wallType = WallType.Full;

    // 空參數建構子，僅設置格子索引
    public HexTile(int x, int y)
    {
        gridPosition = new Vector2Int(x, y);
        worldPosition = Vector3.zero;
        tileType = TileType.None;
    }

    // 完整建構子
    public HexTile(Vector2Int gridPos, Vector3 worldPos)
    {
        gridPosition = gridPos;
        worldPosition = worldPos;
        tileType = TileType.None;
    }

    public void SetAsFloor()
    {
        tileType = TileType.Floor;
        isRoom = true;
        isCorridor = false;
    }

    public void SetAsCorridor()
    {
        tileType = TileType.Corridor;
        isRoom = false;
        isCorridor = true;
    }

    public void SetAsWall(WallType type)
    {
        tileType = TileType.Wall;
        isRoom = false;
        isCorridor = false;
        wallType = type;
    }

    public bool isFloor => tileType == TileType.Floor;
    public bool isWall => tileType == TileType.Wall;
}