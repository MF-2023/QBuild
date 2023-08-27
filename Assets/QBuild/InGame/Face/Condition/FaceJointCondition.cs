using System;
using System.Collections.Generic;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Condition
{
    [CreateAssetMenu(fileName = "FaceJointCondition", menuName = EditorConst.ScriptablePrePath + "FaceJointCondition",
        order = EditorConst.OtherOrder)]
    public class FaceJointCondition : FaceConditionable, ISerializationCallbackReceiver
    {
        public override bool IsExclude(FaceScriptableObject face1, FaceScriptableObject face2)
        {
            if (!_conditionMap.ContainsKey(face1))
            {
                return true;
            }

            if (!_conditionMap[face1].ContainsKey(face2))
            {
                return true;
            }

            return _conditionMap[face1][face2];
        }


        [Serializable]
        private class JointFace
        {
            public FaceScriptableObject FaceA;
            public FaceScriptableObject FaceB;
        }

        [SerializeField] private List<JointFace> _jointFaces;
        [SerializeField] private List<JointFace> _excludeFaces;

        private void OnValidate()
        {
            Debug.Log("FaceJointCondition OnValidate");

            Refresh();
        }

        private Dictionary<FaceScriptableObject, Dictionary<FaceScriptableObject, bool>> _conditionMap = new();

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            Refresh();
        }

        private void Refresh()
        {
            _conditionMap.Clear();
            foreach (var jointFace in _excludeFaces)
            {
                if (!_conditionMap.ContainsKey(jointFace.FaceA))
                {
                    _conditionMap.Add(jointFace.FaceA, new Dictionary<FaceScriptableObject, bool>());
                }

                _conditionMap[jointFace.FaceA].Add(jointFace.FaceB, false);
            }
        }
    }
}