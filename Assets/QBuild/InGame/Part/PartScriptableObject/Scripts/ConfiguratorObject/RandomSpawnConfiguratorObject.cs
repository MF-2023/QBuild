using System;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Part.PartScriptableObject.ConfiguratorObject
{
    [CreateAssetMenu(menuName = EditorConst.ScriptablePrePath + "RandomSpawnConfiguratorObject",
        fileName = "RandomSpawnConfiguratorObject")]
    public class RandomSpawnConfiguratorObject : BasePartSpawnConfiguratorObject
    {
        public override int GetPartObjectCount => _elements.Length;

        [SerializeField] private Element[] _elements;

        public override BlockPartScriptableObject GetPartObject(int hint = -1)
        {
            var randomIndex = UnityEngine.Random.Range(0, _elements.Length);
            return _elements[randomIndex].BlockPartScriptableObject;
        }

        [Serializable]
        private class Element
        {
            [SerializeField] private BlockPartScriptableObject _blockPartScriptableObject;
            [SerializeField] private float _probability;

            public BlockPartScriptableObject BlockPartScriptableObject => _blockPartScriptableObject;
            public float Probability => _probability;
        }
    }
}