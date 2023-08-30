using System.Collections.Generic;
using System.Linq;
using QBuild.Camera;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace QBuild.PropertyWindow
{
    public class PropertyEditorView : VisualElement
    {
        public PropertyEditorView()
        {
            var tree = UIToolkitUtility.GetVisualTree("PropertyWindow/PropertyEditorView");
            var baseElement = tree.Instantiate();
            Add(baseElement);
            Initialize();
        }

        public new class UxmlFactory : UxmlFactory<PropertyEditorView, UxmlTraits>
        {
        }

        public void SetEditItem(ScriptableTreeElement item)
        {
            this.Q<Label>("title-label").text = item.DisplayName;
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{item.Type.Name}");
            if (guids.Length == 0)
            {
                throw new System.IO.FileNotFoundException($"{item.Type.Name} does not found");
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var obj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            _currentScriptableObject = obj;


            if (_inspectorElement != null) Remove(_inspectorElement);
            _inspectorElement = new InspectorElement(_currentScriptableObject);
            Add(_inspectorElement);
        }


        private void Initialize()
        {
            this.Q<Label>("title-label").text = "プロパティエディタ";
        }

        private InspectorElement _inspectorElement;
        private ScriptableObject _currentScriptableObject;
    }
}