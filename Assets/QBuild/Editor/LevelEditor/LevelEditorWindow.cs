using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Gimmick;
using QBuild.Stage;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace QBuild.LevelEditor
{
    public class LevelEditorWindow : EditorWindow
    {
        private VisualTreeAsset _visualTreeAsset;


        private List<EditorPage> _pages;
        private int _tabIndex = -1;
        private Toolbar _toolbar;

        public enum ItemType
        {
            None,
            Root,
            Leaf,
        }

        [Serializable]
        public struct Item
        {
            public ItemType type;
            public string name;
            public GameObject prefab;
        }

        [MenuItem("Tools/QBuild/LevelEditorWindow")]
        private static void Init()
        {
            var window = (LevelEditorWindow)EditorWindow.GetWindow(typeof(LevelEditorWindow));
            window.Show();
        }

        private void OnEnable()
        {
            _pages = new List<EditorPage>()
            {
                new AddGimmick(),
                new AddPart()
            };
            _toolbar = new Toolbar(_pages.ConvertAll(p => p.Title).ToArray());
            _toolbar.OnTabChanged += OnTabChanged;
            _tabIndex = -1;
        }


        private readonly List<TreeViewItemData<Item>> _rootItems = new();

        private void CreateGUI()
        {
            var root = rootVisualElement;
            Debug.Log("CreateGUI");
            _visualTreeAsset = UIToolkitUtility.GetVisualTree("LevelEditor/LevelEditorWindow");
            _visualTreeAsset.CloneTree(root);


            var menu = root.Q<TreeView>("LeftContainer");
            var gimmickPrefabs = FileUtilities.FindInGameAssetsOfType<GameObject>("Gimmick/Prefabs", "Prefab");

            int id = 0;
            var prefabItems = gimmickPrefabs.Select(obj =>
                    new TreeViewItemData<Item>(id++,
                        new Item() { type = ItemType.Leaf, name = obj.name, prefab = obj }))
                .ToList();
            _rootItems.Add(new TreeViewItemData<Item>(id++, new Item()
            {
                type = ItemType.Root,
                name = "ギミック",
            }, prefabItems));

            var partPrefabs = FileUtilities.FindInGameAssetsOfType<GameObject>("Part/Prefab", "Prefab");
            var partItems = partPrefabs.Select(partPrefab => new TreeViewItemData<Item>(id++,
                new Item() { type = ItemType.Leaf, name = partPrefab.name, prefab = partPrefab })).ToList();
            _rootItems.Add(new TreeViewItemData<Item>(id++, new Item()
            {
                type = ItemType.Root,
                name = "パーツ",
            }, partItems));


            menu.SetRootItems(_rootItems);
            menu.makeItem = () =>
            {
                var elementRoot = new VisualElement();
                elementRoot.style.flexDirection = FlexDirection.Row;
                elementRoot.Add(new Label());

                var button = new Button
                {
                    text = "追加"
                };
                button.style.display = DisplayStyle.None;
                elementRoot.Add(button);
                return elementRoot;
            };
            menu.bindItem = (e, i) =>
            {
                var item = menu.GetItemDataForIndex<Item>(i);
                e.Q<Label>().text = menu.GetItemDataForIndex<Item>(i).name;

                Debug.Log(item.name + "を追加ボタンの設定");
                if (item.type != ItemType.Leaf)
                {
                    return;
                }

                e.Q<Button>().style.display = DisplayStyle.Flex;
                e.Q<Button>().clickable = new Clickable(() =>
                {
                    var stageObject = FindObjectOfType<StageBehavior>();
                    var p = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(stageObject);
                    var parent = PrefabUtility.LoadPrefabContents(p);

                    Undo.RecordObject(parent, "Edit Prefab");
                    var obj = PrefabUtility.InstantiatePrefab(item.prefab, parent.transform) as GameObject;

                    PrefabUtility.SaveAsPrefabAsset(parent, p);

                    Undo.RegisterCompleteObjectUndo(parent, "Prefab Change");
                    PrefabUtility.UnloadPrefabContents(parent);
                });
            };
            menu.unbindItem = (e, i) => { e.Q<Button>().style.display = DisplayStyle.None; };
            menu.selectionType = SelectionType.Single;

            var toolbar = root.Q<ToolbarBreadcrumbs>();
        }


        private void OnTabChanged(int tabIndex)
        {
            _tabIndex = tabIndex;
            _pages[_tabIndex].Init();
        }

        private abstract class EditorPage
        {
            public virtual string Title { get; }


            public abstract void Init();
            public abstract void OnGUI();
        }

        private class AddGimmick : EditorPage
        {
            public override string Title => "ギミック追加";

            private IEnumerable<GameObject> _gimmickPrefabs;

            private GameObject _selectedGimmickPrefab = null;

            public override void Init()
            {
                _gimmickPrefabs = FileUtilities.FindInGameAssetsOfType<GameObject>("Gimmick/Prefabs", "Prefab");
                _selectedGimmickPrefab = null;
            }

            public override void OnGUI()
            {
                using (new GUILayout.HorizontalScope())
                {
                    using (new GUILayout.VerticalScope())
                    {
                        GUILayout.Label(Title);
                        foreach (var gimmickPrefab in _gimmickPrefabs)
                        {
                            if (GUILayout.Button(gimmickPrefab.name))
                            {
                                _selectedGimmickPrefab = gimmickPrefab;
                            }
                        }
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.Label("プレビュー");
                }
            }
        }

        private class AddPart : EditorPage
        {
            public override string Title => "パーツ追加";

            public override void Init()
            {
            }

            public override void OnGUI()
            {
                GUILayout.Label(Title);
            }
        }

        private class Toolbar
        {
            private readonly string[] _tabToggles;
            private int _tabIndex;

            public event Action<int> OnTabChanged;


            public Toolbar(string[] tabToggles)
            {
                _tabToggles = tabToggles;
            }

            public void OnGUI()
            {
                GUILayout.BeginHorizontal();
                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    var oldIndex = _tabIndex;
                    _tabIndex = GUILayout.Toolbar(_tabIndex, _tabToggles, new GUIStyle(EditorStyles.toolbarButton),
                        GUI.ToolbarButtonSize.FitToContents);

                    if (oldIndex != _tabIndex) OnTabChanged?.Invoke(_tabIndex);
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}