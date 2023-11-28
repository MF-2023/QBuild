using SoVariableTool.Binding;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SoVariableTool.Bind
{
    [CustomEditor(typeof(Binder))]
    public class BinderCustomEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {

            var container = new VisualElement();

            // IMGUI同様のInspectorを実装
            InspectorElement.FillDefaultInspector(container, serializedObject, this);

            return container;
        }
    }
}