using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 走廊規劃器：負責房間間的連接與符合指定流向的六角螺旋繞圈邏輯。
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

    /// <summary>
    /// 螺旋結果：包含走廊路徑與在路徑上隨機生成的房間中心點
    /// </summary>
    public class SpiralResult
    {
        public List<Vector2Int> CorridorPath = new List<Vector2Int>();
        public List<Vector2Int> RoomCenters = new List<Vector2Int>();
    }

    /// <summary>
    /// 嚴格按照順時鐘方向生成六邊形螺旋走廊。
    /// 走每圈時依序從30°→90°→…→330°，每邊步數等於圈數。
    /// 可在路徑上依機率插分支與房間。
    /// </summary>
    /// <param name="origin">起點（通常地圖中心）</param>
    /// <param name="loops">圈數（螺旋最大半徑）</param>
    /// <param name="branchProbability">分支機率 0~100</param>
    /// <param name="roomProbability">房間插入機率 0~100</param>
    /// <param name="seed">隨機種子（0 表示不固定）</param>
    public SpiralResult GenerateOrderedSpiral(Vector2Int origin, int loops, int branchProbability, int roomProbability, int seed = 0)
    {
        var result = new SpiralResult();
        var rng = seed != 0 ? new System.Random(seed) : new System.Random();
        var current = origin;

        // 逐圈生成：每圈邊長遞增
        for (int r = 1; r <= loops; r++)
        {
            // 六邊形六條邊，順序固定
            for (int dirIndex = 0; dirIndex < DIRECTIONS.Length; dirIndex++)
            {
                var dir = DIRECTIONS[dirIndex];
                for (int step = 0; step < r; step++)
                {
                    current += dir;
                    result.CorridorPath.Add(current);

                    // 隨機分支短線（相同方向 r/2 步左右）
                    if (rng.Next(100) < branchProbability)
                    {
                        int branchLen = Mathf.Max(1, r / 2);
                        var branch = GenerateOrderedBranch(current, branchLen, rng);
                        result.CorridorPath.AddRange(branch);
                    }
                    // 隨機插房間
                    if (rng.Next(100) < roomProbability)
                        result.RoomCenters.Add(current);
                }
            }
        }

        return result;
    }

    // 私有：在當前方向上產生一段短分支，方向隨機選自六方向
    private List<Vector2Int> GenerateOrderedBranch(Vector2Int origin, int length, System.Random rng)
    {
        var path = new List<Vector2Int>();
        var current = origin;
        var dir = DIRECTIONS[rng.Next(DIRECTIONS.Length)];
        for (int i = 0; i < length; i++)
        {
            current += dir;
            path.Add(current);
        }
        return path;
    }
}
