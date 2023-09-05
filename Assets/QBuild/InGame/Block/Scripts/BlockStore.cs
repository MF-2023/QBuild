using System;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    /// <summary>
    /// ブロックを管理するクラス
    /// </summary>
    public class BlockStore
    {
        public void AddBlock(Block block)
        {
            try
            {
                _blockDictionary.Add(block.GetGridPosition(), block);
            }
            catch (Exception e)
            {
                Debug.Log($"Block重複 {_blockDictionary[block.GetGridPosition()].name}");
            }
        }

        public bool TryGetBlock(Vector3Int pos, out Block block)
        {
            var contains = _blockDictionary.ContainsKey(pos);
            block = contains ? _blockDictionary[pos] : null;
            return contains;
        }
        
        public BlockState GetBlockState(Vector3Int pos)
        {
            var contains = _blockDictionary.ContainsKey(pos);
            var state = contains ? _blockDictionary[pos].GetBlockState() : BlockState.NullBlock;
            return state;
        }
        
        
        public bool Contains(Vector3Int pos)
        {
            return _blockDictionary.ContainsKey(pos);
        }

        public void Update(Block block,Vector3Int beforePos)
        {
            RemoveBlock(beforePos);
            AddBlock(block);
        }

        public void Clear()
        {
            _blockDictionary.Clear();
        }

        public void RemoveBlock(Vector3Int pos)
        {
            _blockDictionary.Remove(pos);
        }

        private readonly Dictionary<Vector3Int, Block> _blockDictionary = new();
    }
}