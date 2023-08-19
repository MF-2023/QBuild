using System;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace QBuild
{
    public class BlockFactory
    {
        [Inject]
        public BlockFactory(BlockManager blockManager, BlockCreateInfo createInfo)
        {
            _blockManager = blockManager;
            _blockPrefab = createInfo.Prefab;
        }

        public Block CreateBlock(BlockType blockType, Vector3Int position, Transform parent)
        {
            
            var blockGameObject = Object.Instantiate(_blockPrefab, position, Quaternion.identity, parent);

            if (!blockGameObject.TryGetComponent(out Block block))
            {
                Debug.LogError($"BlockFactory: Prefab.{_blockPrefab.name} に Block がアタッチされていません");
                return null;
            }

            block.GenerateBlock(blockType, position);
            
            OnBlockCreated?.Invoke(block);

            return block;
        }

        public event Action<Block> OnBlockCreated;

        private readonly GameObject _blockPrefab;
        private readonly BlockManager _blockManager;
    }
}