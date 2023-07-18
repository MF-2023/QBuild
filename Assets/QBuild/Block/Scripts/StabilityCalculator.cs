using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace QBuild
{
    [Serializable]
    public class BlockNode
    {
        public float stability { get; set; } = 0;
        public float mass { get; set; } = 0;
        public float totalMass { get; set; } = 0;
        [SerializeField] private Vector3Int[] targetsPos = new Vector3Int[6];
        [SerializeField] private int targetsCount = 0;

        public Vector3Int position { get; set; }

        public void SetTarget(BlockManager blockManager, Block block, Polyomino mino)
        {
            var dirs = new Dictionary<BlockFace, Vector3Int>
            {
                {BlockFace.Top, Vector3Int.up}, {BlockFace.Bottom, Vector3Int.down},
                {BlockFace.South, Vector3Int.forward}, {BlockFace.North, Vector3Int.back},
                {BlockFace.West, Vector3Int.left}, {BlockFace.East, Vector3Int.right}
            };
            foreach (var dir in dirs)
            {
                if (!blockManager.TryGetBlock(block.GetGridPosition() + dir.Value, out var targetBlock)) continue;

                var IsFamily = mino.GetBlocks().Any(minoBlock => targetBlock == minoBlock);
                if (IsFamily) continue;

                targetsPos[(int) dir.Key] = block.GetGridPosition() + dir.Value;
            }
        }

        public IEnumerable<Vector3Int> GetTargets()
        {
            return targetsPos.Where(x => x != Vector3Int.zero);
        }

        [SerializeField] private Vector3Int[] supportPos = new Vector3Int[6];
        [SerializeField] private int supportCount = 0;

        public void SetSupport(Vector3Int pos)
        {
            var face = BlockFace.None;
            if (pos + Vector3Int.up == this.position) face = BlockFace.Bottom;
            if (pos + Vector3Int.down == this.position) face = BlockFace.Top;
            if (pos + Vector3Int.forward == this.position) face = BlockFace.North;
            if (pos + Vector3Int.back == this.position) face = BlockFace.South;
            if (pos + Vector3Int.left == this.position) face = BlockFace.East;
            if (pos + Vector3Int.right == this.position) face = BlockFace.West;
            supportPos[(int) face] = pos;
        }

        public IEnumerable<Vector3Int> GetSupport()
        {
            return supportPos.Where(x => x != Vector3Int.zero);
        }
    }

    [Serializable]
    public class StabilityCalculator
    {
        private BlockManager blockManager;
        private Dictionary<Vector3Int, int> posPlaced = new(20 * 20 * 20);

        private Dictionary<Vector3Int, BlockNode> tree = new();
        [SerializeField] private List<BlockNode> nodes = new List<BlockNode>();

        public void Init(BlockManager bm)
        {
            blockManager = bm;
        }

        public void RegisterMino(Polyomino mino)
        {
            foreach (var block in mino.GetBlocks())
            {
                var node = new BlockNode();
                node.stability = block.GetStabilityGlue();
                node.mass = block.GetMass();
                node.position = block.GetGridPosition();
                node.SetTarget(blockManager, block, mino);
                tree.Add(block.GetGridPosition(), node);
                nodes.Add(node);
                foreach (var target in node.GetTargets())
                {
                    if (!tree.ContainsKey(target)) continue;
                    tree[target].SetSupport(block.GetGridPosition());
                }
            }
        }

        public bool CalcRefresh(Vector3Int startPos)
        {
            var posChecked = new HashSet<Vector3Int>();


            posChecked.Add(startPos);

            if (!tree.ContainsKey(startPos)) return false;
            var startNode = tree[startPos];
            //TODO: Check if the block is supported
            startNode.totalMass = startNode.mass;
            var targets = tree[startPos].GetTargets();

            //Nodeを幅優先探索で探索
            var queue = new Queue<BlockNode>();
            queue.Enqueue(startNode);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (var target in node.GetTargets())
                {
                    if (posChecked.Contains(target)) continue;
                    if (!tree.ContainsKey(target)) continue;
                    var targetNode = tree[target];
                    //NodeのsupportPosからNodeを再帰的に質量を計算する
                    var support = targetNode.GetSupport();
                    var supportMass = 0f;
                    var supportQueue = new Queue<BlockNode>();
                    supportQueue.Enqueue(targetNode);
                    while (supportQueue.Count > 0)
                    {
                        var supportNode = supportQueue.Dequeue();
                        foreach (var supportTarget in supportNode.GetSupport())
                        {
                            if (posChecked.Contains(supportTarget)) continue;
                            if (!tree.ContainsKey(supportTarget)) continue;
                            var supportTargetNode = tree[supportTarget];
                            supportMass += supportTargetNode.mass;
                            supportQueue.Enqueue(supportTargetNode);
                        }
                    }

                    targetNode.totalMass += node.totalMass + targetNode.mass;
                    posChecked.Add(target);
                    queue.Enqueue(targetNode);
                }
            }

            return false;
        }

        private HashSet<Vector3Int> unstablePositions = new HashSet<Vector3Int>();

        private Queue<Vector3Int> positionsToCheck = new Queue<Vector3Int>();

        private Queue<Vector3Int> uniqueUnstablePositions = new Queue<Vector3Int>();

        public List<Vector3Int> CalcPhysicsStabilityToFall(Vector3Int _pos, int maxBlocksToCheck,
            out float calculatedStability)
        {
            List<Vector3Int> list = new List<Vector3Int>();
            calculatedStability = 0f;
            this.unstablePositions.Clear();
            this.unstablePositions.Add(_pos);
            this.positionsToCheck.Clear();
            this.positionsToCheck.Enqueue(_pos);
            this.uniqueUnstablePositions.Clear();
            float glueForce = 0;
            float mass = 0;
            int i = 0;

            var allDirections = new Vector3Int[]
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.forward,
                Vector3Int.right,
                Vector3Int.back,
            };
            while (i < maxBlocksToCheck)
            {
                var force = glueForce;

                foreach (var checkPosition in this.positionsToCheck)
                {
                    if (!blockManager.TryGetBlock(checkPosition, out var block))
                        continue;
                    if (glueForce > 0)
                    {
                        Debug.Log($"blockPosition{checkPosition} {glueForce}", block.gameObject);
                    }
                    var distance = Vector3Int.Distance(new Vector3Int(_pos.x, 0, _pos.z),
                        new Vector3Int(checkPosition.x, 0, checkPosition.z));
                    float scale = 1;
                    if (distance > 1)
                    {
                        scale = 2;
                    }
                    if(distance > 2)
                    {
                        scale = 3;
                    }
                    mass += block.GetMass() * scale;
                    foreach (var direction in allDirections)
                    {
                        var targetPosition = checkPosition + direction;

                        if (targetPosition.y == 0)
                        {
                            continue;
                        }

                        var isAir = !blockManager.TryGetBlock(targetPosition, out var other);
                        if (isAir) continue;
                        //ブロックの安定性
                        var stability = other.GetStability();
                        if ((int) stability == 10)
                        {
                            var forceToOtherBlock = block.GetForceToOtherBlock(other);
                            force += forceToOtherBlock;
                            glueForce += forceToOtherBlock;
                        }
                        else if ((stability > 1) &&
                                 this.unstablePositions.Add(targetPosition))
                        {
                            this.uniqueUnstablePositions.Enqueue(targetPosition);
                            
                            force += block.GetForceToOtherBlock(other);
                        }
                    }
                }

                if (force > 0)
                {
                    calculatedStability = 1f - (float) mass / (float) force;
                }

                if (mass > force)
                {
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