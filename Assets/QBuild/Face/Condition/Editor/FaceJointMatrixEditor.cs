using UnityEditor;

namespace QBuild.Condition
{
    [CustomEditor(typeof(FaceJointMatrix))]
    public class FaceJointMatrixEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            
            var faceJoints = (FaceJointMatrix)target;

            
            EditorGUILayout.Space ();
            
            base.OnInspectorGUI();
        }
    }
}