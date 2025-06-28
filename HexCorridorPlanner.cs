using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 走廊規劃器：負責房間間的直線連接與六角環形繞圈邏輯。
/// </summary>
public class HexCorridorPlanner
{
    /// <summary>無參構造，保持模組化。</summary>
    public HexCorridorPlanner() { }

    /// <summary>
    /// 以簡單直線（八向）依序連接房間中心點，回傳每段走廊的格子座標列表。
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
    /// 計算兩點間的直線格子路徑（簡化版，對角步相連）。
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

    // 六個固定方向，用於 GenerateLoop
    private static readonly Vector2Int[] loopDirections = new Vector2Int[] {
        new Vector2Int(1, 0),   // 30°
        new Vector2Int(0, 1),   // 90°
        new Vector2Int(-1, 1),  // 150°
        new Vector2Int(-1, 0),  // 210° (–150°)
        new Vector2Int(0, -1),  // 270° (–90°)
        new Vector2Int(1, -1)   // 330° (–30°)
    };

    /// <summary>
    /// 從 origin 開始，按六邊形六個邊繞圈，每邊走 lengthPerSide 步，回傳所有路徑座標。
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
