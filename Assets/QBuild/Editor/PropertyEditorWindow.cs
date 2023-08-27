using UnityEditor;
using UnityEngine;

namespace QBuild
{
    public class PropertyEditorWindow : EditorWindow
    {
        [MenuItem(Edito)]
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