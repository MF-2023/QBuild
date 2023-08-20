using System.Collections.Generic;
using System.Linq;
using QBuild.Mino;
using UnityEngine;
using VContainer;

namespace QBuild
{
    /// <summary>
    /// ブロックの安定性を計算するクラス
    /// </summary>
    public class StabilityCalculator
    {
        
        [Inject]
        public StabilityCalculator(BlockStore blockStore)
        {
            _blockStore = blockStore;
        }


        /// <summary>
        /// 落下するブロックを計算して取得する
        /// </summary>
        /// <param name="pos">計算開始地点</param>
        /// <param name="maxBlocksToCheck">最大再帰数</param>
        /// <param name="calculatedStability">戻り値：計算された安定性</param>
        /// <returns>落下するブロックのリスト</returns>
        public List<Vector3Int> CalcPhysicsStabilityToFall(Vector3Int pos, int maxBlocksToCheck,
            out float calculatedStability)
        {
            var list = new List<Vector3Int>();
            calculatedStability = 0f;
            this._unstablePositions.Clear();
            this._unstablePositions.Add(pos);
            this._positionsToCheck.Clear();
            this._positionsToCheck.Enqueue(pos);
            this._uniqueUnstablePositions.Clear();
            float glueForce = 0;
            float mass = 0;
            int i = 0;

            while (i < maxBlocksToCheck)
            {
                var force = glueForce;
                var minoKey = MinoKey.NullMino;
                while (this._positionsToCheck.Count > 0)
                {
                    var checkPosition = this._positionsToCheck.Dequeue();
                    if (!_blockStore.TryGetBlock(checkPosition, out var block))
                        continue;
                    minoKey = block.GetMinoKey();
                    if (glueForce > 0)
                    {
                        Debug.Log($"blockPosition{checkPosition} {glueForce}", block.gameObject);
                    }

                    var distance = Vector3Int.Distance(new Vector3Int(pos.x, 0, pos.z),
                        new Vector3Int(checkPosition.x, 0, checkPosition.z));
                    float scale = 1;
                    if (distance > 1)
                    {
                        scale = 2;
                    }

                    if (distance > 2)
                    {
                        scale = 3;
                    }

                    mass += block.GetMass() * scale;
                    foreach (var direction in Vector3IntDirs.AllDirections)
                    {
                        var targetPosition = checkPosition + direction;

                        var isAir = !_blockStore.TryGetBlock(targetPosition, out var other);
                        if (isAir) continue;
                        var equalMino = minoKey == other.GetMinoKey();
                        //ブロックの安定性
                        var stability = other.GetStability();
                        if ((int)stability == 10)
                        {
                            var forceToOtherBlock = block.GetForceToOtherBlock(other);
                            glueForce += forceToOtherBlock;
                        }

                        if ((int)stability == 10 && !equalMino)
                        {
                            var forceToOtherBlock = block.GetForceToOtherBlock(other);
                            force += forceToOtherBlock;
                        }
                        else if ((stability > 1) &&
                                 this._unstablePositions.Add(targetPosition))
                        {
                            this._uniqueUnstablePositions.Enqueue(targetPosition);
                            Debug.Log(
                                $"unstable {force},{block.GetForceToOtherBlock(other)},{block.GetStabilityGlue()}, {other.GetStabilityGlue()}");
                            force += block.GetForceToOtherBlock(other);

                            if (mass > force && equalMino) this._positionsToCheck.Enqueue(targetPosition);
                        }
                    }
                }

                if (force > 0)
                {
                    calculatedStability = 1f - (float)mass / (float)force;
                }

                if (mass > force)
                {
                    Debug.Log($"{mass},{force}");
                    list = this._unstablePositions.Except(this._uniqueUnstablePositions).ToList<Vector3Int>();
                    if (list.Count == 0)
                    {
                        calculatedStability = 1f;
                        break;
                    }

                    break;
                }
                else
                {
                    if (this._uniqueUnstablePositions.Count == 0)
                    {
                        break;
                    }

                    this._positionsToCheck.Clear();
                    (this._uniqueUnstablePositions, this._positionsToCheck) =
                        (this._positionsToCheck, this._uniqueUnstablePositions);
                    this._uniqueUnstablePositions.Clear();
                    i++;
                }
            }

            return list;
        }

        private readonly BlockStore _blockStore;

        private Dictionary<Vector3Int, int> _posPlaced = new(20 * 20 * 20);

        private readonly HashSet<Vector3Int> _unstablePositions = new();

        private Queue<Vector3Int> _positionsToCheck = new();

        private Queue<Vector3Int> _uniqueUnstablePositions = new();
    }
}