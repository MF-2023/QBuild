using System;
using System.Collections.Generic;
using QBuild.Stage;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace QBuild
{
    /// <summary>
    /// ブロックの振る舞いを定義するクラス
    /// </summary>
    public class BlockService
    {
        [Inject]
        public BlockService(BlockStore blockStore, FaceConditionable conditionMap,
            StageScriptableObject stageScriptableObject)
        {
            _blockStore = blockStore;
            _conditionMap = conditionMap;
            _stageScriptableObject = stageScriptableObject;
        }
        public bool TryGetBlock(Vector3Int pos, out Block block)
        {
            return _blockStore.TryGetBlock(pos, out block);
        }

        public BoundBlockState GetBlockStates(Bounds bounds)
        {
            var min = new Vector3Int((int) Math.Floor(bounds.min.x), (int) Math.Floor(bounds.min.y),
                (int) Math.Floor(bounds.min.z));
            var max = new Vector3Int((int) Math.Floor(bounds.max.x), (int) Math.Floor(bounds.max.y),
                (int) Math.Floor(bounds.max.z));

            var blocks = new List<BlockState>();
            for (var bz = min.z; bz <= max.z; bz++)
            {
                for (var by = min.y; by <= max.y; by++)
                {
                    for (var bx = min.x; bx <= max.x; bx++)
                    {
                        var state = _blockStore.GetBlockState(new Vector3Int(bx, by, bx));
                        blocks.Add(state);
                    }
                }
            }

            return new BoundBlockState(min, max, blocks.ToArray());
        }

        public void UpdateBlock(Block block, Vector3Int beforePosition)
        {
            var pos = block.GetGridPosition();
            if (_blockStore.TryGetBlock(pos, out _))
            {
                _blockStore.RemoveBlock(pos);
            }

            _blockStore.Update(block, beforePosition);
        }

        public bool CanPlace(Vector3Int pos)
        {
            var area = _stageScriptableObject.Size;
            if (pos.x < 0 || pos.x >= area.x || pos.y < 0 || pos.y >= area.y || pos.z < 0 || pos.z >= area.z)
            {
                return false;
            }

            return !_blockStore.TryGetBlock(pos, out _);
        }

        public bool ContactCondition(Block owner, Block other)
        {
            var dir = other.GetGridPosition() - owner.GetGridPosition();
            var faceDirType = dir.ToVectorBlockFace();
            var ownerFace = owner.GetFace(faceDirType);
            var otherFace = other.GetFace(faceDirType.Opposite());
            return _conditionMap.IsExclude(ownerFace.GetFaceType(), otherFace.GetFaceType());
        }

        public void RemoveBlock(Block block)
        {
            _blockStore.RemoveBlock(block.GetGridPosition());
            Object.Destroy(block.gameObject);
        }

        public void Clear()
        {
            _blockStore.Clear();
        }

        public static bool CanJoint(Block owner, Block other)
        {
            var dir = other.GetGridPosition() - owner.GetGridPosition();
            var faceDirType = dir.ToVectorBlockFace();
            var ownerFace = owner.GetFace(faceDirType);
            var otherFace = other.GetFace(faceDirType.Opposite());

            return (ownerFace.GetFaceType().name == "Convex" && otherFace.GetFaceType().name == "Concave") ||
                   (ownerFace.GetFaceType().name == "Concave" && otherFace.GetFaceType().name == "Convex");
        }


        private readonly StageScriptableObject _stageScriptableObject;
        private readonly BlockStore _blockStore;
        private readonly FaceConditionable _conditionMap;
    }
}