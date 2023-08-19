using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace QBuild
{

    [Serializable]
    public class StabilityCalculator
    {
        private BlockManager blockManager;
        private Dictionary<Vector3Int, int> posPlaced = new(20 * 20 * 20);
        
        public void Init(BlockManager bm)
        {
            blockManager = bm;
        }

        private HashSet<Vector3Int> unstablePositions = new();

        private Queue<Vector3Int> positionsToCheck = new();

        private Queue<Vector3Int> uniqueUnstablePositions = new();

        
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
            this.unstablePositions.Clear();
            this.unstablePositions.Add(pos);
            this.positionsToCheck.Clear();
            this.positionsToCheck.Enqueue(pos);
            this.uniqueUnstablePositions.Clear();
            float glueForce = 0;
            float mass = 0;
            int i = 0;
            
            while (i < maxBlocksToCheck)
            {
                var force = glueForce;
                long minoKey = -1;
                while (this.positionsToCheck.Count > 0)
                {
                    var checkPosition = this.positionsToCheck.Dequeue();
                    if (!blockManager.TryGetBlock(checkPosition, out var block))
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

                        var isAir = !blockManager.TryGetBlock(targetPosition, out var other);
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
                                 this.unstablePositions.Add(targetPosition))
                        {

                            this.uniqueUnstablePositions.Enqueue(targetPosition);
                            Debug.Log(
                                $"unstable {force},{block.GetForceToOtherBlock(other)},{block.GetStabilityGlue()}, {other.GetStabilityGlue()}");
                            force += block.GetForceToOtherBlock(other);
                            
                            if (mass > force && equalMino) this.positionsToCheck.Enqueue(targetPosition);
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
                    list = this.unstablePositions.Except(this.uniqueUnstablePositions).ToList<Vector3Int>();
                    if (list.Count == 0)
                    {
                        calculatedStability = 1f;
                        break;
                    }

                    break;
                }
                else
                {
                    if (this.uniqueUnstablePositions.Count == 0)
                    {
                        break;
                    }

                    this.positionsToCheck.Clear();
                    (this.uniqueUnstablePositions, this.positionsToCheck) =
                        (this.positionsToCheck, this.uniqueUnstablePositions);
                    this.uniqueUnstablePositions.Clear();
                    i++;
                }
            }

            return list;
        }
    }
}