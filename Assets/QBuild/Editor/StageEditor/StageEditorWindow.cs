#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Linq;
using QBuild.Const;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace QBuild.StageEditor
{
     public class StageEditorWindow : EditorWindow
    {
        private StageData _editingStageData;
        private const string BlocksInventoryPath = "Assets/QBuild/Editor/StageEditor/Blocks/";
        private const string SaveStageDataFolderPath = "Assets/QBuild/InGame/Stage/StageData";
        private const string StageEditorSceneName = "StageEditor";
        private const string BlockLayerName = "Block";
        private const string IconPath = "Assets/QBuild/Editor/StageEditor/Icons/";
        private const string StageDataScriptableObjectName = "StageData";

        private readonly List<StageData> _stageDataList = new();

        private struct BlockData
        {
            public GameObject prefab;
            public Texture2D thumbnail;
        }

        private readonly List<BlockData> _blockList = new();

        private List<CheckNormalStageData.WarningLog> _warningLogs = new();

        private Texture2D _trashIcon, _saveIcon, _refreshIcon, _newIcon;

        private const string TrashIconName = "Trash.png",
            SaveIconName = "Save.png",
            RefreshIconName = "Refresh.png",
            NewIconName = "New.png";

        [MenuItem(EditorConst.WindowPrePath + "ステージエディタ/ステージエディタウィンドウ")]
        private static void Open()
        {
            var window = GetWindow<StageEditorWindow>();
            window.Show();
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            EditorSceneManager.sceneClosing += (scene, mode) => Initialize();
            Refresh();
            _trashIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath + TrashIconName);
            _saveIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath + SaveIconName);
            _refreshIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath + RefreshIconName);
            _newIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath + NewIconName);
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void Initialize()
        {
            var scene = SceneManager.GetActiveScene();
            
            if ( scene.name != StageEditorSceneName) return;
            
            var obj = GetAllBlocksInScene();
            foreach (var o in obj)
            {
                DestroyImmediate(o);
            }
            
            _editingStageData = null;

            EditorSceneManager.SaveScene(scene);
        }

        private void OnGUI()
        {
            minSize = new Vector2(300, 300);

            if (SceneManager.GetActiveScene().name != StageEditorSceneName)
            {
                OnDrawOtherSceneWarning();
                return;
            }

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

            var area = _editingStageData._stageArea;
            var center = new Vector3(0.0f, area.y / 2.0f, 0.0f);

            Handles.DrawWireCube(center, area);

            var objPos = Selection.gameObjects;
            float grid = 1.0f;
            var distance = 0.05f;

            foreach (var obj in objPos)
            {
                if (obj.layer != LayerMask.NameToLayer(BlockLayerName)) continue;

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

        //別のシーンが開かれているときに呼ばれる
        private void OnDrawOtherSceneWarning()
        {
            EditorGUILayout.HelpBox("ステージエディタシーンを開いてください。", MessageType.Warning);
            if (GUILayout.Button("ステージエディタシーンを開く"))
                StageDataSceneTab.OpenStageEditorScene();
        }

        private void OnDrawDataEdit()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox,
                       GUILayout.Height(40)))
            {
                if (GUILayout.Button(_newIcon, GUILayout.Width(40), GUILayout.Height(40)))
                {
                    CreateStageData("NewStageData");
                }

                bool grayOut = _editingStageData == null;
                using (new EditorGUI.DisabledScope(grayOut))
                {
                    if (GUILayout.Button(_saveIcon, GUILayout.Height(40)))
                    {
                        SaveStageData();
                    }
                }

                if (GUILayout.Button(_refreshIcon, GUILayout.Width(80), GUILayout.Height(40)))
                {
                    Refresh();
                }
            }
        }

        private void Refresh()
        {
            AssetDatabase.Refresh();

            RefreshStageDataList();
            RefreshBlockList();
            ObjectSnapper.stageArea = _editingStageData?._stageArea ?? Vector3Int.zero;
            _warningLogs = new List<CheckNormalStageData.WarningLog>();
            _warningLogs = CheckNormalStageData.CheckStageData(_editingStageData);
        }

        private void RefreshStageDataList()
        {
            _stageDataList.Clear();
            var guids = AssetDatabase.FindAssets("t:" + StageDataScriptableObjectName);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var stageData = AssetDatabase.LoadAssetAtPath<StageData>(path);
                if (stageData == null) continue;
                _stageDataList.Add(stageData);
            }
        }

        private void RefreshBlockList()
        {
            _blockList.Clear();
            var guids = AssetDatabase.FindAssets("t:GameObject", new[] { BlocksInventoryPath });
            foreach (var guid in guids)
            {
                BlockData blockData = new();
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var block = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (block == null) continue;
                blockData.prefab = block;
                blockData.thumbnail = AssetPreview.GetAssetPreview(block);
                _blockList.Add(blockData);
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


                foreach (var stageData in _stageDataList)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUI.color = Color.white;
                        if (GUILayout.Button(_trashIcon, GUILayout.Height(20), GUILayout.Width(20))) //Delete
                        {
                            var result = EditorUtility.DisplayDialog("消去", "本当に消去しますか？",
                                "はい",
                                "いいえ"
                            );

                            if (result)
                            {
                                DeleteStageData(stageData);
                                return;
                            }
                        }

                        if (stageData == _editingStageData) GUI.color = Color.green;
                        if (GUILayout.Button(stageData._fileName, GUILayout.ExpandWidth(true))) //Load
                        {
                            LoadStageData(stageData);
                            return;
                        }
                    }
                }

                GUI.color = Color.white;
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

                    var text = EditorGUILayout.DelayedTextField(_editingStageData._fileName);
                    if (text != null && text != _editingStageData._fileName)
                    {
                        var log = AssetDatabase.RenameAsset(SaveStageDataFolderPath + "/" + _editingStageData._fileName,
                            text);
                        if (log != "")
                        {
                            EditorUtility.DisplayDialog("エラー", "ファイル名が重複しています。", "OK");
                        }
                        else
                        {
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_editingStageData), text);
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(_editingStageData._stage), text);
                            _editingStageData._fileName = text;
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                }


                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Stage name", GUILayout.Width(100));

                    var text = EditorGUILayout.DelayedTextField(_editingStageData._stageName);
                    if (text != null && text != _editingStageData._stageName)
                    {
                        _editingStageData._stageName = text;
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Stage difficult", GUILayout.Width(100));

                    //1~5 slider
                    var difficult = EditorGUILayout.IntSlider(_editingStageData._stageDifficult, 1, 5);
                    if (difficult != _editingStageData._stageDifficult)
                    {
                        _editingStageData._stageDifficult = difficult;
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("Stage area", GUILayout.Width(100));

                    EditorGUILayout.LabelField("x", GUILayout.Width(10));
                    int x = EditorGUILayout.DelayedIntField(_editingStageData._stageArea.x);
                    EditorGUILayout.LabelField("y", GUILayout.Width(10));
                    int y = EditorGUILayout.DelayedIntField(_editingStageData._stageArea.y);
                    EditorGUILayout.LabelField("z", GUILayout.Width(10));
                    int z = EditorGUILayout.DelayedIntField(_editingStageData._stageArea.z);

                    var area = new Vector3Int(x, y, z);

                    if (_editingStageData._stageArea != area)
                    {
                        _editingStageData._stageArea = area;
                        ObjectSnapper.stageArea = area;

                        var obj = GetAllBlocksInScene();
                        foreach (var o in obj) ObjectSnapper.SnapToGrid(o.transform);


                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                }

                {
                    EditorGUILayout.LabelField("Stage image", GUILayout.Width(100));

                    var texture = EditorGUILayout.ObjectField(_editingStageData._stageImage, typeof(Texture), false,
                        GUILayout.ExpandWidth(true),
                        GUILayout.ExpandHeight(true)) as Texture2D;

                    if (_editingStageData._stageImage != texture)
                    {
                        _editingStageData._stageImage = texture;
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
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

                float availableWidth = EditorGUIUtility.currentViewWidth - 35;
                int buttonsPerRow = Mathf.FloorToInt(availableWidth / 100);

                float buttonSize = (availableWidth - (buttonsPerRow - 1)) / buttonsPerRow;

                for (int i = 0; i < _blockList.Count; i++)
                {
                    if (i % buttonsPerRow == 0)
                        EditorGUILayout.BeginHorizontal();

                    var prefab = _blockList[i].prefab;
                    Texture2D thumbnail = _blockList[i].thumbnail;

                    if (GUILayout.Button(thumbnail, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                        InstanceObject(prefab);

                    if ((i + 1) % buttonsPerRow == 0 || i == _blockList.Count - 1)
                        EditorGUILayout.EndHorizontal();
                }
            }
        }

        private const float _InstanceDistance = 5f;

        private void InstanceObject(GameObject prefab)
        {
            if (_editingStageData == null) return;

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

        private Vector2 _stageStatusScrollPosition;

        private void OnDrawStageStatus()
        {
            if (_editingStageData == null) return;

            using (var scroll = new EditorGUILayout.ScrollViewScope(
                       _stageStatusScrollPosition,
                       EditorStyles.helpBox,
                       GUILayout.ExpandWidth(true),
                       GUILayout.MaxHeight(100)))
            {
                _stageStatusScrollPosition = scroll.scrollPosition;

                foreach (var log in _warningLogs)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField(log.text, GUILayout.ExpandWidth(true));
                        if (log.targetObject.Count > 0)
                        {
                            if (GUILayout.Button("Focus", GUILayout.Width(50)))
                            {
                                Selection.objects = log.targetObject.ToArray();
                            }
                        }
                    }
                }
            }
        }

        private void DeleteStageData(StageData stageData)
        {
            AssetDatabase.DeleteAsset(SaveStageDataFolderPath + "/" + stageData._fileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var obj = GetAllBlocksInScene();
            foreach (var o in obj) DestroyImmediate(o);

            Refresh();
        }

        private void SaveStageData()
        {
            if (_editingStageData == null) return;

            ShrinkStageData(_editingStageData);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Refresh();
        }

        private void LoadStageData(StageData stageData)
        {
            GUI.FocusControl("");

            if (_editingStageData == stageData) return;

            if (_editingStageData != null)
            {
                var num = EditorUtility.DisplayDialogComplex("開く", "変更内容を保存しますか？",
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
            ObjectSnapper.stageArea = stageData._stageArea;

            DestroyAllBlocks();

            ExpandStageData(stageData);

            EditorUtility.SetDirty(_editingStageData);

            Refresh();
        }
        private static void DestroyAllBlocks()
        {
            var obj = GetAllBlocksInScene();
            foreach (var o in obj)
            {
                DestroyImmediate(o);
            }
        }
        
        private void ExpandStageDataFromAddressable(StageData stageData)
        {
            if (stageData._stage == null) return;

            Addressables
                .LoadAssetAsync<GameObject>("Example") // アドレスを文字列で指定
                .Completed += op => {
                // 結果を取得してインスタンス化
                // 本来はエラーハンドリングなど必要
                Instantiate(op.Result);
            };
        }

        private void ExpandStageData(StageData stageData)
        {
            if (stageData._stage == null) return;

            var obj = Instantiate(stageData._stage);

            int count = obj.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = obj.transform.GetChild(0);
                child.SetParent(null);
            }

            DestroyImmediate(obj);
        }

        public static List<GameObject> GetAllBlocksInScene()
        {
            var objects = FindObjectsOfType<GameObject>();

            return objects.Where(obj => obj.layer == LayerMask.NameToLayer(BlockLayerName)).ToList();
        }

        private void ShrinkStageData(StageData stageData)
        {
            var parent = new GameObject("Stage");
            bool isExistSaveData = false;

            var obj = GetAllBlocksInScene();
            foreach (var o in obj)
            {
                isExistSaveData = true;
                o.transform.SetParent(parent.transform);
            }

            if (!isExistSaveData)
            {
                DestroyImmediate(parent);
                return;
            }

            var prefab =
                PrefabUtility.SaveAsPrefabAsset(parent,
                    SaveStageDataFolderPath + "/" + stageData._fileName + "/" + stageData._fileName + ".prefab");

            stageData._stage = prefab;

            foreach (var child in obj)
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
            stageData._fileName = fileName + i;
            stageData._stageName = "";
            stageData._stageArea = new Vector3Int(10, 10, 10);
            AssetDatabase.CreateAsset(stageData, fullFileName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Refresh();
        }
    }
}