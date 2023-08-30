using QBuild.Const;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QBuild.PropertyWindow
{
    public class PropertyEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        [MenuItem(EditorConst.WindowPrePath + "プロパティエディタ")]
        public static void ShowExample()
        {
            var wnd = GetWindow<PropertyEditorWindow>();
            wnd.titleContent = new GUIContent("PropertyEditorWindow");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            var root = rootVisualElement;
            
            m_VisualTreeAsset.CloneTree(root);
            
            
            Debug.Log("Bind PropertyEditorView");
            var view = root.Q<PropertyEditorView>();
            var list = root.Q<PropertyEditorList>();

            list.OnSelectItem += view.SetEditItem;
        }
    }
}
