using System;
using System.Collections.Generic;
using QBuild.Const;
using UnityEngine;

namespace QBuild.DebugSystem
{
    
    [Serializable]
    public class PlacedMinoInfo
    {
        [SerializeField] private Vector3Int _position;
        [SerializeField] private MinoType _minoType;

        public MinoType MinoType => _minoType;
        public Vector3Int Position => _position;
    }
    
    
    [CreateAssetMenu(fileName = EditorConst.ScriptablePrePath + "MinoStageTemplate", menuName = "MinoStageTemplate", order = EditorConst.OtherOrder)]
    public class MinoStageTemplate : ScriptableObject
    {
        [SerializeField] private List<PlacedMinoInfo> _placedMinoInfos;
        
        public IEnumerable<PlacedMinoInfo> GetPlacedMinoInfos()
        {
            return _placedMinoInfos;
        }
    }
}