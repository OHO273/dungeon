using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HexMapVisualizer))]
public class HexMap : MonoBehaviour
{
    [Header("Map Settings")]
    public int mapWidth = 60;
    public int mapHeight = 60;
    public float hexRadius = 0.5f;
    public int seed = 0;

    [Header("Room Settings")]
    public int maxRooms = 10;
    public int roomMinSize = 4;
    public int roomMaxSize = 8;
    public int roomMaxAttempts = 100;

    [Header("Prefabs")]
    public GameObject floorPrefab;
    public GameObject[] wallPrefabs;

    private HexMapData data;
    private HexRoomGenerator roomGenerator;
    private HexCorridorPlanner corridorPlanner;
    private HexCorridorGenerator corridorGenerator;
    private HexMapVisualizer visualizer;

    void Start()
    {
        BuildMap();
    }

    public void BuildMap()
    {
        Random.InitState(seed);
        // �M���¦a��
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        // ? ��l�Ʀa�ϸ�� & �ж�
        data = new HexMapData(mapWidth, mapHeight, hexRadius, seed);
        roomGenerator = new HexRoomGenerator(seed);
        List<HexRoom> rooms = roomGenerator.GenerateRooms(
            data, maxRooms, roomMinSize, roomMaxSize, roomMaxAttempts);

        // ? ���Y�W���P�ͦ�
        corridorPlanner = new HexCorridorPlanner();        // �L�Ѽƫغc
        var corridors = corridorPlanner.PlanCorridors(rooms); // �Y�A�n�ۭq PlanCorridors�A�Ϊ����ե� GenerateLoop
        corridorGenerator = new HexCorridorGenerator();
        corridorGenerator.GenerateCorridors(corridors, data);

        // ? �X�X �b�o�̴��J�u���δ`���v�޿� �X�X
        var center = new Vector2Int(mapWidth / 2, mapHeight / 2);
        int loopSize = 10;
        var loopCoords = corridorPlanner.GenerateLoop(center, loopSize);
        foreach (var pos in loopCoords)
        {
            data.GetTile(pos.x, pos.y).SetAsCorridor();
        }
        // �]�i��^��ѤU�S�аO�� None ���]��
        for (int x = 0; x < data.width; x++)
            for (int y = 0; y < data.height; y++)
                if (data.GetTile(x, y).tileType == TileType.None)
                    data.GetTile(x, y).SetAsWall(WallType.Full);
        // �X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X�X

        // ? �̫��V
        visualizer = GetComponent<HexMapVisualizer>();
        visualizer.Visualize(data.tiles, hexRadius, floorPrefab, wallPrefabs, transform);
    }

}