using QBuild.Condition;
using QBuild.Stage;
using UnityEngine;
using VContainer;

namespace QBuild
{
    /// <summary>
    /// ブロックの振る舞いを定義するクラス
    /// </summary>
    public class BlockUseCase
    {
        [Inject]
        public BlockUseCase(BlockStore blockStore, FaceJointMatrix conditionMap,
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
            return _conditionMap.GetCondition(ownerFace.GetFaceType(), otherFace.GetFaceType());
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
        private readonly FaceJointMatrix _conditionMap;
    }
}