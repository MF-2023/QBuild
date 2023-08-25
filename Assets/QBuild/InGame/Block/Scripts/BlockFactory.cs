using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild
{
    public class BlockFactory
    {
        [Inject]
        public BlockFactory(BlockCreateInfo createInfo,IObjectResolver resolver)
        {
            _blockPrefab = createInfo.Prefab;
            _resolver = resolver;
        }

        public Block CreateBlock(BlockType blockType, Vector3Int position, Transform parent)
        {
            
            var blockGameObject = _resolver.Instantiate(_blockPrefab, position, Quaternion.identity, parent);

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
        private readonly IObjectResolver _resolver;
    }
}