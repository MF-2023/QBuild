#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using QBuild.Const;
using Unity.VisualScripting;
using UnityEngine;

namespace QBuild.StageEditor
{
    public class StageEditorWindow : EditorWindow
    {
        private StageData _editingStageData;
        private const string StageDataSavePath = "Assets/QBuild/Editor/StageEditor/";
        private const string BlocksInventoryPath = "Assets/QBuild/Editor/StageEditor/Blocks/";

        [MenuItem(EditorConst.WindowPrePath + "StageEditor/StageEditorWindow")]
        private static void Open()
        {
            var window = GetWindow<StageEditorWindow>();
            window.Show();
        }

        private void OnGUI()
        {
            minSize = new Vector2(300, 300);

            OnDrawDataEdit();
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            using (new EditorGUILayout.HorizontalScope())
            {
                OnDrawStageScriptableObjects();
                OnDrawStageData();
            }

            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            OnDrawInventory();

            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            OnDrawStageStatus();
        }

        private void OnDrawDataEdit()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox,
                       GUILayout.Height(40)))
            {
                if (GUILayout.Button("New", GUILayout.Width(40), GUILayout.ExpandHeight(true)))
                {
                    CreateStageData("NewStageData");
                }

                if (GUILayout.Button("Save", GUILayout.ExpandHeight(true)))
                {
                    SaveStageData();
                }
            }
        }

        private Vector2 _dataScrollPosition;

        private void OnDrawStageScriptableObjects()
        {
            using (var scroll =
                   new GUILayout.ScrollViewScope(_dataScrollPosition, EditorStyles.helpBox, GUILayout.Width(150),
                       GUILayout.Height(250)))
            {
                _dataScrollPosition = scroll.scrollPosition;

                var guids = AssetDatabase.FindAssets("t:StageData");
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var stageData = AssetDatabase.LoadAssetAtPath<StageData>(path);
                    if (stageData == null) continue;

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("×", GUILayout.Width(20)))
                        {
                            DeleteStageData(stageData);
                            return;
                        }

                        if (GUILayout.Button(stageData.name, GUILayout.Width(30), GUILayout.ExpandWidth(true)))
                        {
                            LoadStageData(stageData);
                            EditorGUIUtility.PingObject(stageData);
                        }
                    }
                }
            }
        }

        private void OnDrawStageData()
        {
            if (_editingStageData == null) return;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Height(250)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("File name", GUILayout.Width(100));

                    var text = EditorGUILayout.TextField(_editingStageData.fileName);
                    _editingStageData.fileName = text;
                    _editingStageData.name = text;
                }


                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Stage name", GUILayout.Width(100));

                    var text = EditorGUILayout.TextField(_editingStageData.stageName);
                    _editingStageData.stageName = text;
                }

                /*
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Crystal count", GUILayout.Width(100));

                    var count = EditorGUILayout.TextField(_editingStageData.crystalCount.ToString());
                    _editingStageData.crystalCount = int.Parse(count);
                }
                */

                {
                    //TODO:横幅に合わせてLabelFieldのアスペクト比が必ず16:9になるようにする
                    EditorGUILayout.LabelField("Stage image", GUILayout.Width(100));

                    var texture = EditorGUILayout.ObjectField(_editingStageData.stageImage, typeof(Texture), false,
                        GUILayout.ExpandWidth(true),
                        GUILayout.ExpandHeight(true)) as Texture;

                    _editingStageData.stageImage = texture;
                }
            }
        }

        private Vector2 _inventoryScrollPosition;

        private void OnDrawInventory()
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(
                       _inventoryScrollPosition,
                       EditorStyles.helpBox,
                       GUILayout.ExpandWidth(true),
                       GUILayout.ExpandHeight(true)))
            {
                _inventoryScrollPosition = scroll.scrollPosition;

                string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { BlocksInventoryPath　});
                string[] paths = guids.Select(AssetDatabase.GUIDToAssetPath).ToArray();

                float availableWidth = EditorGUIUtility.currentViewWidth - 35;
                int buttonsPerRow = Mathf.FloorToInt(availableWidth / 100);

                float buttonSize = (availableWidth - (buttonsPerRow - 1)) / buttonsPerRow;

                for (int i = 0; i < paths.Length; i++)
                {
                    if (i % buttonsPerRow == 0)
                        EditorGUILayout.BeginHorizontal();

                    var path = paths[i];
                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab == null) continue;

                    Texture2D thumbnail = AssetPreview.GetAssetPreview(prefab);

                    if (GUILayout.Button(thumbnail, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                        InstanceObject(prefab);

                    if ((i + 1) % buttonsPerRow == 0 || i == paths.Length - 1)
                        EditorGUILayout.EndHorizontal();
                }
            }
        }

        private const float _InstanceDistance = 5f;
        private void InstanceObject(GameObject prefab)
        {
            var position = SceneView.lastActiveSceneView.camera.transform.position +
                           SceneView.lastActiveSceneView.camera.transform.forward * _InstanceDistance;
            var rotation = Quaternion.identity;

            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance != null)
            {
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                ObjectSnapper.SnapToGrid(instance.transform);
            }
            
        }

        private void OnDrawStageStatus()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Height(100),
                       GUILayout.ExpandWidth(true)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    //TODO:ステージの状態を表示する
                    //スタート地点が設置されているか
                    //ゴール地点が設置されているか
                    //オブジェクトの原点がズレていないか
                    //オブジェクトの数
                    //クリスタルの数
                }
            }
        }

        private void DeleteStageData(StageData stageData)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(stageData));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void SaveStageData()
        {
            Debug.Log("Save");
            if (_editingStageData == null) return;
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void LoadStageData(StageData stageData)
        {
            Debug.Log("Load");
            _editingStageData = stageData;
        }

        private void CreateStageData(string fileName)
        {
            int i = 1;

            while (AssetDatabase.LoadAssetAtPath<StageData>(StageDataSavePath + fileName + i + ".asset") != null)
                i++;

            var fullFileName = StageDataSavePath + fileName + i + ".asset";

            var stageData = CreateInstance<StageData>();
            stageData.fileName = fileName + i;
            AssetDatabase.CreateAsset(stageData, fullFileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}