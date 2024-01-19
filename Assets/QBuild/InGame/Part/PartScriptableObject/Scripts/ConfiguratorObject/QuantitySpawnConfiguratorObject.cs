using System;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Part.PartScriptableObject.ConfiguratorObject
{
    [CreateAssetMenu(menuName = EditorConst.ScriptablePrePath + "QuantitySpawnConfiguratorObject",
        fileName = "QuantitySpawnConfiguratorObject")]
    public class QuantitySpawnConfiguratorObject : BasePartSpawnConfiguratorObject
    {
        public override int GetPartObjectCount => _elements.Length;

        [SerializeField] private Element[] _elements;
        
        public override BlockPartScriptableObject GetPartObject(int hint = -1)
        {
            var element = GetElement(hint);
            return element.BlockPartScriptableObject;
        }
        
        public int GetQuantity(int index)
        {
            var element = GetElement(index);
            return element.Quantity;
        }
        

        private Element GetElement(int hint = -1)
        {
            if (hint < 0 || hint >= _elements.Length)
            {
                Debug.LogError("hint is out of range");
                return null;
            }

            return _elements[hint];
        }
        
        [Serializable]
        private class Element
        {
            [SerializeField] private BlockPartScriptableObject _blockPartScriptableObject;
            [SerializeField] private int _quantity;

            public BlockPartScriptableObject BlockPartScriptableObject => _blockPartScriptableObject;
            public int Quantity => _quantity;
        }
    }
}