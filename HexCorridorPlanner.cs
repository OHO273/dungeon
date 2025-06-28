using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Y�W�����G�t�d�ж��������u�s���B��������¶��P�ýu�ͦ��޿�C
/// </summary>
public class HexCorridorPlanner
{
    // ��������V�G30�X, 90�X, 150�X, 210�X, 270�X, 330�X
    private static readonly Vector2Int[] DIRECTIONS = {
        new Vector2Int(1, 0),    // 30�X
        new Vector2Int(0, 1),    // 90�X
        new Vector2Int(-1, 1),   // 150�X
        new Vector2Int(-1, 0),   // 210�X
        new Vector2Int(0, -1),   // 270�X
        new Vector2Int(1, -1)    // 330�X
    };

    public HexCorridorPlanner() { }

    /// <summary>
    /// ���u�s���ж����ߡA�^�ǨC�q���Y����l�y�ЦC��C
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

    // ²�������u���|�]����V�̱��񪽽u���s���^
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
    /// ¶�������G�q origin �}�l�A������V�A�C�䨫 lengthPerSide �B�C
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
    /// �ýu�����G�����H�����B�A�O���b maxRadius �d�򤺡C
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
                // �W�X�d��N�Ϥ�V
                next = current - dir;
            }
            current = next;
            path.Add(current);
        }
        return path;
    }

    /// <summary>
    /// ���ۡ���u���X�G�̧ǼW�[�b�|�ͦ��u��������v�A�æb�L�{���H������P�ͦ��ж��C
    /// ��^���ۨ��Y�M�ж����ߦC��C
    /// </summary>
    public SpiralResult GenerateSpiral(WindConfig config)
    {
        var rng = config.seed != 0 ? new System.Random(config.seed) : new System.Random();
        var result = new SpiralResult();
        var current = config.origin;
        for (int r = 1; r <= config.loops; r++)
        {
            // �C�����@�ӥb�| r ������
            for (int d = 0; d < DIRECTIONS.Length; d++)
            {
                var dir = DIRECTIONS[d];
                for (int i = 0; i < r; i++)
                {
                    current += dir;
                    result.Corridors.Add(current);

                    // �H���ͦ�����
                    if (rng.Next(100) < config.branchChance)
                    {
                        var branch = GenerateWindingLoop(current, rng.Next(1, r + 1), r / 2, rng.Next());
                        result.Corridors.AddRange(branch);
                    }
                    // �H���ͦ��ж�
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
    /// �t�m�ѼơG���۰�ơB������v�B�ж����v���C
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