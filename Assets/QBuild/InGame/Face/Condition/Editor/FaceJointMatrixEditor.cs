using System.Collections.Generic;
using System.Linq;
using SherbetInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace QBuild.Condition
{
    [CustomEditor(typeof(FaceJointMatrix), false)]
    public class FaceJointMatrixEditor : ExtensionInspector
    {
        int selectPage = 0;

        public override void OnInspectorGUI()
        {
            var faceJoints = (FaceJointMatrix)target;

            var tables = faceJoints.GetFaceTypes();
            if (faceJoints.GetMatrix().Count() != tables.Count())
            {
                faceJoints.Refresh();
            }

            var conditionFaces = faceJoints.GetMatrix();

            EditorGUILayout.LabelField("Face Joint Matrix", EditorStyles.boldLabel);
            selectPage = EditorGUILayout.Popup("Sample", selectPage, tables.Select(x => x.name).ToArray());
            var selectedFace = tables[selectPage];
            if (!conditionFaces.ContainsKey(selectedFace))
            {
                faceJoints.Refresh();
            }
            var conditionMap = conditionFaces[selectedFace];
            foreach (var faceScriptableObject in tables)
            {
                var toggle =
                    EditorGUILayout.Toggle(faceScriptableObject.name, conditionMap[faceScriptableObject]);
                if(toggle != conditionMap[faceScriptableObject])
                {
                    faceJoints.SetCondition(selectedFace, faceScriptableObject, toggle);
                    faceJoints.SetCondition(faceScriptableObject, selectedFace, toggle);
                    
                }
            }

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}