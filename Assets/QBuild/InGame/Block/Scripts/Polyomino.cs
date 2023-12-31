﻿using System;
using System.Collections;
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

        private long dictionaryKey;

        public static void Init(BlockManager manager)
        {
            _blockManager = manager;
        }

        public void SetDictionaryKey(long key)
        {
            dictionaryKey = key;
        }


        public long GetDictionaryKey()
        {
            return dictionaryKey;
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


            if (shouldMove)
            {
                foreach (var block in _blocks)
                {
                    block.MoveNext(move);
                }
            }

            foreach (var block in _blocks)
            {
                foreach (var pos in dirs.Select(x => x + block.GetGridPosition()))
                {
                    if (!_blockManager.TryGetBlock(pos, out var dirBlock)) continue;
                    if (dirBlock.IsFalling()) continue;
                    if(!_blockManager.ContactCondition(block, dirBlock)) continue;

                    Place();
                    break;
                }

                if (!isFalling) break;
            }
        }

        private void Place()
        {
            var dirs = new Vector3Int[]
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, -1, 0),
                new Vector3Int(0, 1, 0)
            };
            float stability = -1;
            foreach (var block in _blocks)
            {
                foreach (var dir in dirs)
                {
                    var targetPos = block.GetGridPosition() + dir;
                    if(targetPos.y <= 0) continue;
                    if (!_blockManager.TryGetBlock(targetPos, out var targetBlock)) continue;
                    if (targetBlock.IsFalling()) continue;
                    if (targetBlock.GetMinoKey() == block.GetMinoKey()) continue;
                    if (!_blockManager.TryGetMino(targetBlock.GetMinoKey(), out var otherMino)) continue;
                    if(!_blockManager.ContactTest(block,targetBlock)) continue;
                    otherMino.ContactMino(this);
                    isFalling = false;
                    return;
                }
            }

            foreach (var block in _blocks)
            {
                block.OnBlockPlaced(stability);
                stability = block.GetStability();
            }

            isFalling = false;
        }

        public void ContactMino(Polyomino otherMino)
        {
            var material = _blocks[0].GetComponentInChildren<Renderer>().material;
            foreach (var block in otherMino.GetBlocks())
            {
                block.GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.material = material);
                if (isFalling) continue;
                block.OnBlockPlaced(_blocks[0].GetStability());
                block.SetMinoKey(dictionaryKey);
            }
            _blockManager.RemoveMino(otherMino.GetDictionaryKey());
        }

        public List<Vector3Int> GetProvisionalPlacePosition()
        {
            var dirs = new Vector3Int[]
            {
                new Vector3Int(1, 0, 0),
                new Vector3Int(-1, 0, 0),
                new Vector3Int(0, 0, 1),
                new Vector3Int(0, 0, -1),
                new Vector3Int(0, -1, 0)
            };

            int checkRowMin = int.MinValue;

            foreach (var block in _blocks)
            {
                var empty = false;
                var checkRow = 0;
                do
                {
                    var blockPos = block.GetGridPosition() + new Vector3Int(0, checkRow, 0);
                    foreach (var pos in dirs.Select(x => x + blockPos))
                    {
                        if (!_blockManager.TryGetBlock(pos, out var dirBlock)) continue;
                        if (dirBlock.IsFalling()) continue;
                        if (checkRowMin < checkRow) checkRowMin = checkRow;
                        empty = true;
                        break;
                    }

                    checkRow--;
                } while (!empty);
            }

            return _blocks.Select(block => block.GetGridPosition() + new Vector3Int(0, checkRowMin, 0)).ToList();
        }

        public IReadOnlyList<Block> GetBlocks()
        {
            return _blocks;
        }
    }
}