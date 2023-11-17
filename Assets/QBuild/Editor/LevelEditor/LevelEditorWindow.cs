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
        private Toolbar _toolbar;

        [MenuItem("Window/LevelEditorWindow")]
        private static void Init()
        {
            var window = (LevelEditorWindow) EditorWindow.GetWindow(typeof(LevelEditorWindow));
            window.Show();
        }

        private void OnEnable()
        {
            _toolbar = new Toolbar();
            _toolbar.OnTabChanged += OnTabChanged;
        }

        private void OnGUI()
        {
            _toolbar.OnGUI();
        }
        
        private void OnTabChanged(int tabIndex)
        {
        }

        private class Toolbar
        {
            private readonly string[] _tabToggles = {"ギミック追加", "パーツ追加"};
            private int _tabIndex;

            public event Action<int> OnTabChanged;

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