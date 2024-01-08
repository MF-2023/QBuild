using UnityEditor;
using UnityEngine;

namespace QBuild.StageEditor
{
    [InitializeOnLoad]
    public class ObjectSnapper : Editor
    {
        private const float SnapDistance = 1.0f;
        public static Vector3Int stageArea;

        static ObjectSnapper()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var selectedGameObject = Selection.transforms;

            foreach (var transform in selectedGameObject)
            {
                if (transform.gameObject.layer == LayerMask.NameToLayer("Block"))
                {
                    SnapToGrid(transform);
                }
            }
        }

        public static void SnapToGrid(Transform transform)
        {
            var pos = transform.position;
            var snapPos = new Vector3(
                Mathf.Round(pos.x / SnapDistance) * SnapDistance,
                Mathf.Round(pos.y / SnapDistance) * SnapDistance,
                Mathf.Round(pos.z / SnapDistance) * SnapDistance
            );

            Vector3Int area;
            area = stageArea == Vector3Int.zero ? new Vector3Int(100, 100, 100) : stageArea;
            
            //snapPos‚ðstageArea‚Ì”ÍˆÍ“à‚ÉŽû‚ß‚é
            snapPos.x = Mathf.Clamp(snapPos.x, -area.x / 2.0f, area.x / 2.0f);
            snapPos.y = Mathf.Clamp(snapPos.y, 0, area.y);
            snapPos.z = Mathf.Clamp(snapPos.z, -area.z / 2.0f, area.z / 2.0f);
            
            transform.position = snapPos;
        }
    }
}