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
        public static bool isEnable = true;
        public static bool isEnableArea = true;
        public static bool isBlockOnly = true;

        private static readonly SnapBehaviorObject SnapBehaviorObject;

        private static Vector3 _prevPos;


        static ObjectSnapper()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            var path = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:SnapBehaviorObject")[0]);
            SnapBehaviorObject = AssetDatabase.LoadAssetAtPath<SnapBehaviorObject>(path);
        }

        private void OnEnable()
        {
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var selectedGameObject = Selection.transforms;

            if (selectedGameObject == null) return;

            //selectedGameObject�̒�����StageEditorWall���擾
            var wallList = selectedGameObject
                .Where(tran => tran.TryGetComponent(out Wall wall))
                .ToList();
            if (wallList.Count != 0)
            {
                //selectedGameObject����wallList�����O
                selectedGameObject = selectedGameObject.Except(wallList).ToArray();
                Selection.objects = selectedGameObject.Select(tran => tran.gameObject).ToArray();
            }

            bool doCheckGenerateWallFromPole = false;
            foreach (var tran in selectedGameObject)
            {
                var isCursorLock = tran.TryGetComponent(out Wall wall);

                var p = _prevPos;
                CheckSnapBehaviorObject(tran.gameObject);

                if (isBlockOnly && tran.gameObject.layer != LayerMask.NameToLayer("Block"))
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

            //���b�V�����Ȃ��ꍇ�̓X�L�b�v
            if (mesh == null || selectedMesh == null)
                return false;

            //���_�����Ⴄ�ꍇ�̓X�L�b�v
            if (mesh.vertices.Length != selectedMesh.vertices.Length)
                return false;

            //���_�̈ʒu���Ⴄ�ꍇ�̓X�L�b�v
            if (mesh.vertices.Where((t, i) => t != selectedMesh.vertices[i]).Any())
                return false;

            return true;
        }

        public static void SnapToGrid(Transform transform)
        {
            var pos = transform.position;

            var snapPos = pos;

            if (isEnable)
            {
                //1m���ƂɃX�i�b�v
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