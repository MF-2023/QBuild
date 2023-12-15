using System;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Part.PartScriptableObject.ConfiguratorObject
{
    [CreateAssetMenu(menuName = EditorConst.ScriptablePrePath + "QuantitySpawnConfiguratorObject",
        fileName = "QuantitySpawnConfiguratorObject")]
    public class QuantitySpawnConfiguratorObject: BasePartSpawnConfiguratorObject
    {
        [Serializable]
        private class Element
        {
            [SerializeField] private BlockPartScriptableObject _blockPartScriptableObject;
            [SerializeField] private float _probability;
            
            public BlockPartScriptableObject BlockPartScriptableObject => _blockPartScriptableObject;
            public float Probability => _probability;
        }

        [SerializeField] private Element[] _elements;

        public override BlockPartScriptableObject GetPartObject(int hint = -1)
        {
            if (hint < 0 || hint >= _elements.Length)
            {
                Debug.LogError("hint is out of range");
                return null;
            }
            return _elements[hint].BlockPartScriptableObject;
        }
    }
}