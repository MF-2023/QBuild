using QBuild.Const;
using UnityEditor;
using UnityEngine;

namespace QBuild
{
    public class PropertyEditorWindow : EditorWindow
    {
        [MenuItem(EditorConst.WindowPrePath + "プロパティエディタ")]
        private static void ShowWindow()
        {
            var window = GetWindow<PropertyEditorWindow>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        private void CreateGUI()
        {
            
        }
    }
}