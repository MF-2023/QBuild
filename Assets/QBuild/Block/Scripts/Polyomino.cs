using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QBuild
{
    [Serializable]
    public class Polyomino
    {
        [SerializeField] private List<Block> _blocks = new List<Block>();

        public bool isFalling { get; private set; } = true;

        private static BlockManager _blockManager;

        public static void Init(BlockManager manager)
        {
            _blockManager = manager;
        }

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }

        public void MoveNext(int x, int y, int z)
        {
            MoveNext(new Vector3Int(x, y, z));
        }

        public void MoveNext(Vector3Int move)
        {
            var dirs = new Vector3Int[]
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, -1, 0)
            };
            var shouldMove = _blocks.All(block => block.CanMove(move));

            

            foreach (var block in _blocks)
            {
                if (shouldMove)
                {
                    block.MoveNext(move);
                }

                foreach (var pos in dirs.Select(x => x + block.GetGridPosition()))
                {
                    if (!_blockManager.TryGetBlock(pos, out var dirBlock)) continue;
                    if (dirBlock.IsFalling()) continue;

                    Place();
                    break;
                }
            }
        }

        private void Place()
        {
            foreach (var block in _blocks)
            {
                block.OnBlockPlaced();
            }

            
            isFalling = false;
        }
    }
}