using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SherbetInspector.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace QBuild.Condition
{
    [CreateAssetMenu(fileName = "FaceJointMatrix", menuName = "Tools/QBuild/Face", order = 0)]
    public class FaceJointMatrix : ScriptableObject
    {
        [HideInInspector] public IReadOnlyList<FaceScriptableObject> faceScriptableObjects;

        [Button]
        public void Refresh()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:FaceScriptableObject");
            if (guids.Length == 0)
            {
                throw new System.IO.FileNotFoundException("FaceScriptableObject does not found");
            }


            faceScriptableObjects = guids.Select(guid =>
                AssetDatabase.LoadAssetAtPath<FaceScriptableObject>(AssetDatabase.GUIDToAssetPath(guid))).ToList();
            

        }
    }
}