using System;
using System.Collections.Generic;
using QBuild.Const;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild
{
    [CreateAssetMenu(fileName = "MinoType", menuName = EditorConst.ScriptablePrePath + "MinoType",
        order = 0)]
    public class MinoType : ScriptableObject
    {
        [Serializable]
        public class PositionToBlockGenerator
        {
            [SerializeField] public Vector3Int _pos;
            [SerializeField] public BlockType _blockType;
        }

        [SerializeField] private List<PositionToBlockGenerator> _blockTypes;

        public IReadOnlyList<PositionToBlockGenerator> GetBlockTypes()
        {
            return _blockTypes;
        }
    }
}