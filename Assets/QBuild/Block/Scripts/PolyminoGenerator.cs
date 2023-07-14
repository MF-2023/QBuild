using System;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{

    
    [CreateAssetMenu(fileName = "PolyminoGenerator", menuName = "Tools/QBuild/PolyminoGenerator", order = 0)]
    public class PolyminoGenerator : ScriptableObject
    {
        [Serializable]
        public class PositionToBlockGenerator
        {
            [SerializeField] public Vector3Int pos;
            [SerializeField] public BlockGenerator blockGenerator;
        }

        [SerializeField] private List<PositionToBlockGenerator> _blockGenerators;

        public IReadOnlyList<PositionToBlockGenerator> GetBlockGenerators()
        {
            return _blockGenerators;
        }
    }
}