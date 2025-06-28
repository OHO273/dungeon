using System.Collections.Generic;

public class HexRoomGenerator
{
    private System.Random rng;

    public HexRoomGenerator(int seed)
    {
        rng = new System.Random(seed);
    }

    public List<HexRoom> GenerateRooms(HexMapData data, int maxRooms, int minSize, int maxSize, int maxAttempts)
    {
        var rooms = new List<HexRoom>();
        int attempts = 0;
        int id = 0;

        while (attempts < maxAttempts && rooms.Count < maxRooms)
        {
            attempts++;
            int sizeX = rng.Next(minSize, maxSize + 1);
            int sizeY = rng.Next(minSize, maxSize + 1);
            int startX = rng.Next(0, data.width - sizeX);
            int startY = rng.Next(0, data.height - sizeY);

            bool overlap = false;
            for (int x = startX; x < startX + sizeX; x++)
            {
                for (int y = startY; y < startY + sizeY; y++)
                {
                    if (data.tiles[x, y].tileType != TileType.None)
                    {
                        overlap = true;
                        break;
                    }
                }
                if (overlap) break;
            }
            if (overlap) continue;

            var room = new HexRoom(id++);
            for (int x = startX; x < startX + sizeX; x++)
            {
                for (int y = startY; y < startY + sizeY; y++)
                {
                    var tile = data.tiles[x, y];
                    tile.SetAsFloor();
                    tile.roomID = room.id;
                    room.tiles.Add(tile);
                }
            }
            rooms.Add(room);
        }
        return rooms;
    }
}