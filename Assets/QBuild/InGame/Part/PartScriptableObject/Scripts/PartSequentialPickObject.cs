using System.Collections.Generic;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Part
{
    [CreateAssetMenu(fileName = "PartSequentialOperator",
        menuName = EditorConst.ScriptablePrePath + "PartSequentialPickObject",
        order = 2)]
    public class PartSequentialPickObject : PartPickObject,ISerializationCallbackReceiver
    {
        [SerializeField] private List<BlockPartScriptableObject> _parts = new();
        [SerializeField] private int _currentPartIndex = 0;
        
        public override BlockPartScriptableObject Processing(IEnumerable<GeneratePartInfo> partInfos,
            PickCalcParameter calcParameter)
        {
            if (_parts.Count == 0)
            {
                Debug.LogError("パーツが設定されていません", this);
                return null;
            }
            
            var part = _parts[_currentPartIndex];
            _currentPartIndex++;
            if (_currentPartIndex >= _parts.Count)
            {
                _currentPartIndex = 0;
            }
            return part;
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            _currentPartIndex = 0;
        }
    }
}