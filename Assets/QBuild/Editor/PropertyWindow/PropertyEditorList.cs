using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Camera;
using UnityEngine;
using UnityEngine.UIElements;

namespace QBuild.PropertyWindow
{
    public class PropertyEditorList : VisualElement
    {
        public event Action<ScriptableTreeElement> OnSelectItem; 
        
        public PropertyEditorList()
        {
            var tree = UIToolkitUtility.GetVisualTree("PropertyWindow/PropertyEditorList");
            var baseElement = tree.Instantiate();
            Add(baseElement);
            Initialize();
        }

        public new class UxmlFactory : UxmlFactory<PropertyEditorList, UxmlTraits>
        {
        }

        private void Initialize()
        {
            List<TreeViewItemData<IPropertyElement>> rootItems = new();
            var id = 0;

            var cameraElement =
                new TreeViewItemData<IPropertyElement>(id++, new ScriptableTreeElement(typeof(CameraScriptableObject)));
            rootItems.Add(new TreeViewItemData<IPropertyElement>(id++, new GenreTreeElement("Camera"),
                new List<TreeViewItemData<IPropertyElement>> { cameraElement }));

            var treeView = this.Q<TreeView>();

            treeView.SetRootItems(rootItems);

            treeView.makeItem = () => new Label();
            treeView.bindItem = (element, index) =>
                ((Label)element).text = treeView.GetItemDataForIndex<IPropertyElement>(index).DisplayName;
            treeView.selectionType = SelectionType.Single;
            treeView.selectionChanged += selections =>
            {
                var selected = selections.FirstOrDefault();
                if (selected == null)
                {
                    return;
                }

                switch (selected)
                {
                    case GenreTreeElement genreTreeElement:
                        Debug.Log("Genre:" + genreTreeElement.DisplayName);
                        break;
                    case ScriptableTreeElement scriptableTreeElement:
                        Debug.Log("Scriptable:" + scriptableTreeElement.DisplayName);
                        OnSelectItem?.Invoke(scriptableTreeElement);
                        break;
                }
            };
        }
    }
}