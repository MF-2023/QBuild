using UnityEditor;
using UnityEngine;

namespace QBuild.StageEditor
{
    [InitializeOnLoad]
    public class ObjectSnapper : Editor
    {
        
        private const float SnapDistance = 1.0f;
        
        static ObjectSnapper()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var selectedGameObject = Selection.transforms;

            foreach (var transform in selectedGameObject)
                SnapToGrid(transform);
        }

        public static void SnapToGrid(Transform transform)
        {
            var position = transform.position;
            position.x = Mathf.Round(position.x / SnapDistance) * SnapDistance;
            position.y = Mathf.Round(position.y / SnapDistance) * SnapDistance;
            position.z = Mathf.Round(position.z / SnapDistance) * SnapDistance;
            transform.position = position;
        }
    }
}