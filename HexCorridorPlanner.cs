using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 走廊規劃器：負責房間間的直線連接、六角環形繞圈與亂線生成邏輯。
/// </summary>
public class HexCorridorPlanner
{
    // 對應六方向：30°, 90°, 150°, 210°, 270°, 330°
    private static readonly Vector2Int[] DIRECTIONS = {
        new Vector2Int(1, 0),    // 30°
        new Vector2Int(0, 1),    // 90°
        new Vector2Int(-1, 1),   // 150°
        new Vector2Int(-1, 0),   // 210°
        new Vector2Int(0, -1),   // 270°
        new Vector2Int(1, -1)    // 330°
    };

    public HexCorridorPlanner() { }

    /// <summary>
    /// 直線連接房間中心，回傳每段走廊的格子座標列表。
    /// </summary>
    public List<List<Vector2Int>> PlanCorridors(List<HexRoom> rooms)
    {
        var all = new List<List<Vector2Int>>();
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            var a = rooms[i].Center;
            var b = rooms[i + 1].Center;
            all.Add(GetStraightPath(a, b));
        }
        return all;
    }

    // 簡易的直線路徑（六方向最接近直線的連接）
    private List<Vector2Int> GetStraightPath(Vector2Int start, Vector2Int end)
    {
        var path = new List<Vector2Int>();
        var current = start;
        while (current != end)
        {
            Vector2Int best = current;
            float minDist = float.MaxValue;
            foreach (var dir in DIRECTIONS)
            {
                var next = current + dir;
                float d = (next - end).sqrMagnitude;
                if (d < minDist)
                {
                    minDist = d;
                    best = next;
                }
            }
            current = best;
            path.Add(current);
        }
        return path;
    }

    /// <summary>
    /// 繞圈環路：從 origin 開始，按六方向，每邊走 lengthPerSide 步。
    /// </summary>
    public List<Vector2Int> GenerateLoop(Vector2Int origin, int lengthPerSide)
    {
        var path = new List<Vector2Int>();
        var current = origin;
        for (int i = 0; i < DIRECTIONS.Length; i++)
        {
            var dir = DIRECTIONS[i];
            for (int step = 0; step < lengthPerSide; step++)
            {
                current += dir;
                path.Add(current);
            }
        }
        return path;
    }

    /// <summary>
    /// 亂線環路：類似隨機漫步，保持在 maxRadius 範圍內。
    /// </summary>
    public List<Vector2Int> GenerateWindingLoop(Vector2Int origin, int steps, int maxRadius, int seed = 0)
    {
        var path = new List<Vector2Int>();
        var current = origin;
        var rng = seed != 0 ? new System.Random(seed) : new System.Random();
        for (int i = 0; i < steps; i++)
        {
            var dir = DIRECTIONS[rng.Next(DIRECTIONS.Length)];
            var next = current + dir;
            if ((next - origin).sqrMagnitude > maxRadius * maxRadius)
            {
                // 超出範圍就反方向
                next = current - dir;
            }
            current = next;
            path.Add(current);
        }
        return path;
    }

    /// <summary>
    /// 螺旋＆毛線結合：依序增加半徑生成「螺旋環圈」，並在過程中隨機分支與生成房間。
    /// 返回螺旋走廊和房間中心列表。
    /// </summary>
    public SpiralResult GenerateSpiral(WindConfig config)
    {
        var rng = config.seed != 0 ? new System.Random(config.seed) : new System.Random();
        var result = new SpiralResult();
        var current = config.origin;
        for (int r = 1; r <= config.loops; r++)
        {
            // 每完成一個半徑 r 的環圈
            for (int d = 0; d < DIRECTIONS.Length; d++)
            {
                var dir = DIRECTIONS[d];
                for (int i = 0; i < r; i++)
                {
                    current += dir;
                    result.Corridors.Add(current);

                    // 隨機生成分支
                    if (rng.Next(100) < config.branchChance)
                    {
                        var branch = GenerateWindingLoop(current, rng.Next(1, r + 1), r / 2, rng.Next());
                        result.Corridors.AddRange(branch);
                    }
                    // 隨機生成房間
                    if (rng.Next(100) < config.roomChance)
                    {
                        result.RoomCenters.Add(current);
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 配置參數：螺旋圈數、分支機率、房間機率等。
    /// </summary>
    public class WindConfig
    {
        public Vector2Int origin;
        public int loops;
        public int branchChance;    // 0~100
        public int roomChance;      // 0~100
        public int seed;
    }

    public class SpiralResult
    {
        public List<Vector2Int> Corridors = new List<Vector2Int>();
        public List<Vector2Int> RoomCenters = new List<Vector2Int>();
    }
}