using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEditor.Toolbars;
using Object = UnityEngine.Object;

namespace QBuild.LevelEditor
{
    public class LevelEditorWindow : EditorWindow
    {
        private List<EditorPage> _pages;
        private int _tabIndex = -1;
        private Toolbar _toolbar;


        [MenuItem("Window/LevelEditorWindow")]
        private static void Init()
        {
            var window = (LevelEditorWindow) EditorWindow.GetWindow(typeof(LevelEditorWindow));
            window.Show();
        }

        private void OnEnable()
        {
            _pages = new List<EditorPage>()
            {
                new AddGimmick()
            };
            _toolbar = new Toolbar(_pages.ConvertAll(p => p.Title).ToArray());
            _toolbar.OnTabChanged += OnTabChanged;
        }

        private void OnGUI()
        {
            _toolbar.OnGUI();
            if (_tabIndex < 0) _pages[_tabIndex].OnGUI();
        }

        private void OnTabChanged(int tabIndex)
        {
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

            public override void Init()
            {
            }

            public override void OnGUI()
            {
                GUILayout.Label(Title);
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