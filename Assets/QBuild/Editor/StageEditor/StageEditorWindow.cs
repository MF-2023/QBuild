#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using QBuild.Const;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QBuild.StageEditor
{
    public class StageEditorWindow : EditorWindow
    {
        private StageData _editingStageData;
        private const string BlocksInventoryPath = "Assets/QBuild/Editor/StageEditor/Blocks/";
        private const string SaveStageDataFolderPath = "Assets/QBuild/InGame/Stage/StageData";

        [MenuItem(EditorConst.WindowPrePath + "StageEditor/StageEditorWindow")]
        private static void Open()
        {
            var window = GetWindow<StageEditorWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            minSize = new Vector2(300, 300);

            OnDrawDataEdit();
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            using (new EditorGUILayout.HorizontalScope())
            {
                OnDrawStageScriptableObjects();
                OnDrawStageDataValue();
            }

            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            OnDrawInventory();

            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            OnDrawStageStatus();
        }


        private void OnSceneGUI(SceneView sceneView)
        {
            //Render the area of the stage
            if (_editingStageData == null) return;

            var area = _editingStageData.stageArea;
            var center = new Vector3(0.0f, area.y / 2.0f, 0.0f);

            Handles.DrawWireCube(center, area);

            var objPos = Selection.gameObjects;
            float grid = 1.0f;
            var distance = 0.05f;

            foreach (var obj in objPos)
            {
                if (obj.layer != LayerMask.NameToLayer("Block")) continue;

                for (int x = area.x / 2; x > -area.x / 2; x--)
                {
                    for (int z = area.z / 2; z > -area.z / 2; z--)
                    {
                        var pos = new Vector3(x - grid / 2.0f, obj.transform.position.y, z - grid / 2.0f);

                        Handles.DrawLine(pos + Vector3.left * distance, pos + Vector3.right * distance);
                        Handles.DrawLine(pos + Vector3.up * distance, pos + Vector3.down * distance);
                        Handles.DrawLine(pos + Vector3.forward * distance, pos + Vector3.back * distance);
                    }
                }
            }
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
                            _editingStageData = null;
                            return;
                        }

                        if (GUILayout.Button(stageData.name, GUILayout.Width(30), GUILayout.ExpandWidth(true)))
                        {
                            LoadStageData(stageData);
                            //EditorGUIUtility.PingObject(stageData);
                        }
                    }
                }
            }
        }

        private void OnDrawStageDataValue()
        {
            if (_editingStageData == null) return;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Height(250)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("File name", GUILayout.Width(100));

                    var text = EditorGUILayout.DelayedTextField(_editingStageData.fileName);
                    if (text != null && text != _editingStageData.fileName)
                    {
                        var log = AssetDatabase.RenameAsset(SaveStageDataFolderPath + "/" + _editingStageData.fileName,
                            text);
                        if (log != "")
                            EditorUtility.DisplayDialog("エラー", "ファイル名が重複しています。", "OK");
                        else
                        {
                            Debug.Log(text);
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_editingStageData), text);
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_editingStageData.stage), text);
                            _editingStageData.fileName = text;
                            EditorUtility.SetDirty(_editingStageData);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                            EditorSceneManager.SaveOpenScenes();
                        }

                        GUI.FocusControl("");
                    }
                }


                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Stage name", GUILayout.Width(100));

                    var text = EditorGUILayout.DelayedTextField(_editingStageData.stageName);
                    if (text != null && text != _editingStageData.stageName)
                        _editingStageData.stageName = text;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Stage area", GUILayout.Width(100));

                    var area = EditorGUILayout.Vector3IntField("", _editingStageData.stageArea);
                    if (_editingStageData.stageArea != area)
                    {
                        _editingStageData.stageArea = area;
                        ObjectSnapper.stageArea = area;
                        
                        var objects = FindObjectsOfType<GameObject>();
                        foreach (var obj in objects)
                        {
                            if (obj.layer == LayerMask.NameToLayer("Block"))
                                ObjectSnapper.SnapToGrid(obj.transform);
                        }
                    }
                }

                {
                    EditorGUILayout.LabelField("Stage image", GUILayout.Width(100));

                    var texture = EditorGUILayout.ObjectField(_editingStageData.stageImage, typeof(Texture), false,
                        GUILayout.ExpandWidth(true),
                        GUILayout.ExpandHeight(true)) as Texture;

                    if (_editingStageData.stageImage != texture)
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

                string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { BlocksInventoryPath });
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
            
            if ( _editingStageData == null) return;
            
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
                using (new EditorGUILayout.VerticalScope())
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("StartPoint", GUILayout.Width(100));
                        EditorGUILayout.LabelField("false");
                    }
                 
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField("GoalPoint", GUILayout.Width(100));
                        EditorGUILayout.LabelField("false");
                    }
                }
            }
        }

        private void DeleteStageData(StageData stageData)
        {
            AssetDatabase.DeleteAsset(SaveStageDataFolderPath + "/" + stageData.fileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            var objects = FindObjectsOfType<GameObject>();
            foreach (var obj in objects)
            {
                if (obj.layer == LayerMask.NameToLayer("Block"))
                    DestroyImmediate(obj);
            }
        }

        private void SaveStageData()
        {
            if (_editingStageData == null) return;

            ShrinkStageData(_editingStageData);

            EditorUtility.SetDirty(_editingStageData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void LoadStageData(StageData stageData)
        {
            GUI.FocusControl("");

            if (_editingStageData == stageData) return;

            if (_editingStageData != null)
            {
                var num = EditorUtility.DisplayDialogComplex("エラー", "変更内容を保存しますか？",
                    "はい",
                    "キャンセル",
                    "保存せずに開く"
                );
                switch (num)
                {
                    case 0:
                        SaveStageData();
                        break;
                    case 1:
                        return;
                    case 2:
                        break;
                }
            }

            _editingStageData = stageData;
            ObjectSnapper.stageArea = stageData.stageArea;

            var objects = FindObjectsOfType<GameObject>();
            foreach (var obj in objects)
            {
                if (obj.layer == LayerMask.NameToLayer("Block"))
                    DestroyImmediate(obj);
            }

            ExpandStageData(stageData);
        }

        private void ExpandStageData(StageData stageData)
        {
            if (stageData.stage == null) return;

            var obj = Instantiate(stageData.stage);

            int count = obj.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                obj.transform.GetChild(0).SetParent(null);
            }
            DestroyImmediate(obj);
        }

        private void ShrinkStageData(StageData stageData)
        {
            var objects = FindObjectsOfType<GameObject>();
            var parent = new GameObject("Stage");
            bool isExistSaveData = false;
            foreach (var obj in objects)
            {
                if (obj.layer == LayerMask.NameToLayer("Block"))
                {
                    isExistSaveData = true;
                    obj.transform.SetParent(parent.transform);
                }
            }

            if (!isExistSaveData)
            {
                DestroyImmediate(parent);
                return;
            }

            var prefab =
                PrefabUtility.SaveAsPrefabAsset(parent,
                    SaveStageDataFolderPath + "/" + stageData.fileName + "/" + stageData.fileName + ".prefab");

            stageData.stage = prefab;

            foreach (var child in objects)
                child.transform.SetParent(null);

            DestroyImmediate(parent);
        }

        private void CreateStageData(string fileName)
        {
            int i = 1;

            while (AssetDatabase.IsValidFolder(SaveStageDataFolderPath + "/" + fileName + i))
                i++;

            var numberingFileName = fileName + i;
            var folderPath = SaveStageDataFolderPath + "/" + numberingFileName;

            if (!AssetDatabase.IsValidFolder(folderPath))
                AssetDatabase.CreateFolder(SaveStageDataFolderPath, numberingFileName);

            var fullFileName = folderPath + "/" + numberingFileName + ".asset";

            var stageData = CreateInstance<StageData>();
            stageData.fileName = fileName + i;
            stageData.stageArea = new Vector3Int(10, 10, 10);
            AssetDatabase.CreateAsset(stageData, fullFileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}