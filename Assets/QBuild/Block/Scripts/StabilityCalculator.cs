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
                { BlockFace.Top, Vector3Int.up }, { BlockFace.Bottom, Vector3Int.down },
                { BlockFace.South, Vector3Int.forward }, { BlockFace.North, Vector3Int.back },
                { BlockFace.West, Vector3Int.left }, { BlockFace.East, Vector3Int.right }
            };
            foreach (var dir in dirs)
            {
                if (!blockManager.TryGetBlock(block.GetGridPosition() + dir.Value, out var targetBlock)) continue;

                var IsFamily = mino.GetBlocks().Any(minoBlock => targetBlock == minoBlock);
                if (IsFamily) continue;

                targetsPos[(int)dir.Key] = block.GetGridPosition() + dir.Value;
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
            supportPos[(int)face] = pos;
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
                node.stability = block.GetStability();
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
            var allDirections = new Vector3Int[]
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.forward,
                Vector3Int.right,
                Vector3Int.back,
            };

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
        
    }
}