using UnityEngine;
using VContainer;

namespace QBuild
{
    public class BlockFactory
    {
        [Inject]
        public BlockFactory(BlockManager blockManager, GameObject blockPrefab)
        {
            _blockManager = blockManager;
            _blockPrefab = blockPrefab;
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
            return block;
        }

        private readonly GameObject _blockPrefab;
        private readonly BlockManager _blockManager;
    }
}