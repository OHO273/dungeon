using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Y�W�����G�t�d�ж��������u�s���P��������¶���޿�C
/// </summary>
public class HexCorridorPlanner
{
    /// <summary>�L�Ѻc�y�A�O���ҲդơC</summary>
    public HexCorridorPlanner() { }

    /// <summary>
    /// �H²�檽�u�]�K�V�^�̧ǳs���ж������I�A�^�ǨC�q���Y����l�y�ЦC��C
    /// </summary>
    public List<List<Vector2Int>> PlanCorridors(List<HexRoom> rooms)
    {
        var corridors = new List<List<Vector2Int>>();
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            var start = rooms[i].Center;
            var end = rooms[i + 1].Center;
            corridors.Add(GetStraightPath(start, end));
        }
        return corridors;
    }

    /// <summary>
    /// �p����I�������u��l���|�]²�ƪ��A�﨤�B�۳s�^�C
    /// </summary>
    private List<Vector2Int> GetStraightPath(Vector2Int start, Vector2Int end)
    {
        var path = new List<Vector2Int>();
        var current = start;
        while (current != end)
        {
            var delta = end - current;
            var step = new Vector2Int(
                delta.x == 0 ? 0 : (delta.x > 0 ? 1 : -1),
                delta.y == 0 ? 0 : (delta.y > 0 ? 1 : -1)
            );
            current += step;
            path.Add(current);
        }
        return path;
    }

    // ���өT�w��V�A�Ω� GenerateLoop
    private static readonly Vector2Int[] loopDirections = new Vector2Int[] {
        new Vector2Int(1, 0),   // 30�X
        new Vector2Int(0, 1),   // 90�X
        new Vector2Int(-1, 1),  // 150�X
        new Vector2Int(-1, 0),  // 210�X (�V150�X)
        new Vector2Int(0, -1),  // 270�X (�V90�X)
        new Vector2Int(1, -1)   // 330�X (�V30�X)
    };

    /// <summary>
    /// �q origin �}�l�A������Τ�����¶��A�C�䨫 lengthPerSide �B�A�^�ǩҦ����|�y�СC
    /// </summary>
    public List<Vector2Int> GenerateLoop(Vector2Int origin, int lengthPerSide)
    {
        var path = new List<Vector2Int>();
        var current = origin;
        foreach (var dir in loopDirections)
        {
            for (int step = 0; step < lengthPerSide; step++)
            {
                current += dir;
                path.Add(current);
            }
        }
        return path;
    }
}
