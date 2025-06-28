using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Y�W�����G�t�d�ж������s���P�ŦX���w�y�V����������¶���޿�C
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

    /// <summary>
    /// ���۵��G�G�]�t���Y���|�P�b���|�W�H���ͦ����ж������I
    /// </summary>
    public class SpiralResult
    {
        public List<Vector2Int> CorridorPath = new List<Vector2Int>();
        public List<Vector2Int> RoomCenters = new List<Vector2Int>();
    }

    /// <summary>
    /// �Y����Ӷ�������V�ͦ���������ۨ��Y�C
    /// ���C��ɨ̧Ǳq30�X��90�X���K��330�X�A�C��B�Ƶ����ơC
    /// �i�b���|�W�̾��v������P�ж��C
    /// </summary>
    /// <param name="origin">�_�I�]�q�`�a�Ϥ��ߡ^</param>
    /// <param name="loops">��ơ]���۳̤j�b�|�^</param>
    /// <param name="branchProbability">������v 0~100</param>
    /// <param name="roomProbability">�ж����J���v 0~100</param>
    /// <param name="seed">�H���ؤl�]0 ��ܤ��T�w�^</param>
    public SpiralResult GenerateOrderedSpiral(Vector2Int origin, int loops, int branchProbability, int roomProbability, int seed = 0)
    {
        var result = new SpiralResult();
        var rng = seed != 0 ? new System.Random(seed) : new System.Random();
        var current = origin;

        // �v��ͦ��G�C��������W
        for (int r = 1; r <= loops; r++)
        {
            // ����Τ�����A���ǩT�w
            for (int dirIndex = 0; dirIndex < DIRECTIONS.Length; dirIndex++)
            {
                var dir = DIRECTIONS[dirIndex];
                for (int step = 0; step < r; step++)
                {
                    current += dir;
                    result.CorridorPath.Add(current);

                    // �H������u�u�]�ۦP��V r/2 �B���k�^
                    if (rng.Next(100) < branchProbability)
                    {
                        int branchLen = Mathf.Max(1, r / 2);
                        var branch = GenerateOrderedBranch(current, branchLen, rng);
                        result.CorridorPath.AddRange(branch);
                    }
                    // �H�����ж�
                    if (rng.Next(100) < roomProbability)
                        result.RoomCenters.Add(current);
                }
            }
        }

        return result;
    }

    // �p���G�b��e��V�W���ͤ@�q�u����A��V�H����ۤ���V
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
