using QBuild.Const;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QBuild.PropertyWindow
{
    public class PropertyEditorWindow : EditorWindow
    {
        private VisualTreeAsset m_VisualTreeAsset;

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

            m_VisualTreeAsset = UIToolkitUtility.GetVisualTree("PropertyWindow/PropertyEditorWindow");
            m_VisualTreeAsset.CloneTree(root);
            
            
            Debug.Log("Bind PropertyEditorView");
            var view = root.Q<PropertyEditorView>();
            var list = root.Q<PropertyEditorList>();

            list.OnSelectItem += view.SetEditItem;
        }
    }
}
