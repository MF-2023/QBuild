using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace QBuild.Stage
{
    /// <summary>
    /// ステージの生成を行うクラス
    /// </summary>
    public class StageFactory
    {
        [Inject]
        public StageFactory(StageScriptableObject stageScriptableObject, BlockType planeBlockType,
            BlockFactory blockFactory)
        {
            _stageScriptableObject = stageScriptableObject;
            _planeBlockType = planeBlockType;
            _blockFactory = blockFactory;
        }

        /// <summary>
        /// ステージの生成
        /// </summary>
        /// <param name="blockPrefab">ブロックのプレハブ</param>
        /// <param name="floorParent">ステージを生成する際の親オブジェクト</param>
        public IEnumerable<Block> CreateFloor(GameObject blockPrefab,
            GameObject floorParent)
        {
            var result = new List<Block>();
            for (var x = 0; x < _stageScriptableObject.Width; x++)
            {
                for (var z = 0; z < _stageScriptableObject.Depth; z++)
                {
                    var position = new Vector3Int(x, 0, z);
                    var block = _blockFactory.CreateBlock(_planeBlockType, position, floorParent.transform);
                    if (block == null) continue;
                    block.OnBlockPlaced();
                    block.name = $"floor {position}";

                    result.Add(block);
                }
            }

            return result;
        }

        private readonly StageScriptableObject _stageScriptableObject;
        private readonly BlockType _planeBlockType;
        private readonly BlockFactory _blockFactory;
    }
}