using UnityEditor;
using UnityEngine;

namespace QBuild.StageEditor
{
    [InitializeOnLoad]
    public class ObjectSnapper : Editor
    {
        private const float SnapDistance = 1.0f;
        public static Vector3Int stageArea;
        public static bool isEnable = true;
        public static bool isEnableArea = true;
        public static bool isBlockOnly = true;

        static ObjectSnapper()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var selectedGameObject = Selection.transforms;

            foreach (var transform in selectedGameObject)
            {
                if (isBlockOnly)
                {
                    if (transform.gameObject.layer != LayerMask.NameToLayer("Block"))
                    {
                        continue;
                    }
                }

                SnapToGrid(transform);
            }
        }

        public static void SnapToGrid(Transform transform)
        {
            var pos = transform.position;
            
            var snapPos = pos;

            if (isEnable)
            {
                snapPos = new Vector3(
                    Mathf.Round(pos.x / SnapDistance) * SnapDistance,
                    Mathf.Round(pos.y / SnapDistance) * SnapDistance,
                    Mathf.Round(pos.z / SnapDistance) * SnapDistance
                );
            }

            if (isEnableArea)
            {
                snapPos.x = Mathf.Clamp(snapPos.x, -stageArea.x / 2.0f, stageArea.x / 2.0f);
                snapPos.y = Mathf.Clamp(snapPos.y, 0, stageArea.y);
                snapPos.z = Mathf.Clamp(snapPos.z, -stageArea.z / 2.0f, stageArea.z / 2.0f);
            }

            transform.position = snapPos;
        }
    }
}