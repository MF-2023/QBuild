using System;
using System.Linq;
using QBuild.Gimmick;
using UnityEditor;
using UnityEngine;

namespace QBuild.StageEditor
{
    [InitializeOnLoad]
    public class ObjectSnapper : Editor
    {
        private const float SnapDistance = 1.0f;

        public static float GetSnapDistance()
        {
            return SnapDistance;
        }

        private static float _snapOffset = 0.0f;
        public static Vector3Int stageArea;

        private static bool _isEnable;

        public static bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                EditorPrefs.SetBool("ObjectSnapper.isEnable", value);
                Debug.Log(value);
            }
        }

        public static bool isEnableArea = true;

        private static bool _isBlockOnly;

        public static bool IsBlockOnly
        {
            get => _isBlockOnly;
            set
            {
                _isBlockOnly = value;
                EditorPrefs.SetBool("ObjectSnapper.isBlockOnly", value);
                Debug.Log(value);
            }
        }

        private static readonly SnapBehaviorObject SnapBehaviorObject;

        private static Vector3 _prevPos;


        static ObjectSnapper()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            var path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:SnapBehaviorObject")[0]);
            SnapBehaviorObject = AssetDatabase.LoadAssetAtPath<SnapBehaviorObject>(path);
            _isEnable = EditorPrefs.GetBool("ObjectSnapper.isEnable", true);
            _isBlockOnly = EditorPrefs.GetBool("ObjectSnapper.isBlockOnly", true);
            Debug.Log(EditorPrefs.GetBool("ObjectSnapper.isEnable", true));
        }

        private void OnEnable()
        {
   
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var selectedGameObject = Selection.transforms;

            if (selectedGameObject == null) return;

            //selectedGameObjectの中からStageEditorWallを取得
            var wallList = selectedGameObject
                .Where(tran => tran.TryGetComponent(out Wall wall))
                .ToList();
            if (wallList.Count != 0)
            {
                //selectedGameObjectからwallListを除外
                selectedGameObject = selectedGameObject.Except(wallList).ToArray();
                Selection.objects = selectedGameObject.Select(tran => tran.gameObject).ToArray();
            }

            bool doCheckGenerateWallFromPole = false;
            foreach (var tran in selectedGameObject)
            {
                var isCursorLock = tran.TryGetComponent(out Wall wall);

                var p = _prevPos;
                CheckSnapBehaviorObject(tran.gameObject);

                if (_isBlockOnly && tran.gameObject.layer != LayerMask.NameToLayer("Block"))
                    continue;

                if (isCursorLock)
                {
                    tran.position = wall.position;
                }
                else
                {
                    SnapToGrid(tran);
                }

                if (p != _prevPos)
                {
                    doCheckGenerateWallFromPole = true;
                }
            }

            if (doCheckGenerateWallFromPole)
            {
                StageEditorWindow.WallInitialize();
                StageEditorWindow.CheckGenerateWallFromPole();
            }
        }

        public static void CheckSnapBehaviorObject(GameObject obj)
        {
            _snapOffset = 0.0f;
            var meshFilter = obj.GetComponent<MeshFilter>();
            foreach (var snapBehaviorObject in SnapBehaviorObject.GetSnapBehaviorObjects())
            {
                if (IsExistSnapBehaviorObject(snapBehaviorObject, meshFilter))
                {
                    _snapOffset = 0.5f;
                    break;
                }
            }
        }

        private static bool IsExistSnapBehaviorObject(GameObject obj, MeshFilter selectedObj)
        {
            var meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter == null || selectedObj == null)
                return false;

            var mesh = meshFilter.sharedMesh;
            var selectedMesh = selectedObj.sharedMesh;

            //メッシュがない場合はスキップ
            if (mesh == null || selectedMesh == null)
                return false;

            //頂点数が違う場合はスキップ
            if (mesh.vertices.Length != selectedMesh.vertices.Length)
                return false;

            //頂点の位置が違う場合はスキップ
            if (mesh.vertices.Where((t, i) => t != selectedMesh.vertices[i]).Any())
                return false;

            return true;
        }

        public static void SnapToGrid(Transform transform)
        {
            var pos = transform.position;

            var snapPos = pos;

            if (_isEnable)
            {
                //1mごとにスナップ
                snapPos = new Vector3(
                    Mathf.Round(pos.x / SnapDistance - _snapOffset) * SnapDistance + _snapOffset,
                    Mathf.Round(pos.y / SnapDistance) * SnapDistance,
                    Mathf.Round(pos.z / SnapDistance - _snapOffset) * SnapDistance + _snapOffset
                );
            }

            if (isEnableArea)
            {
                snapPos.x = Mathf.Clamp(
                    snapPos.x,
                    -stageArea.x / 2.0f + _snapOffset,
                    stageArea.x / 2.0f - _snapOffset
                );
                snapPos.y = Mathf.Clamp(snapPos.y, 0, stageArea.y);
                snapPos.z = Mathf.Clamp(
                    snapPos.z,
                    -stageArea.z / 2.0f + _snapOffset,
                    stageArea.z / 2.0f - _snapOffset
                );
            }

            transform.position = snapPos;
            _prevPos = snapPos;
        }
    }
}