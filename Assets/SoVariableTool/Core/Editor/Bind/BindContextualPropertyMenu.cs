using UnityEditor;
using UnityEngine;

namespace SoVariableTool.Bind
{
    [InitializeOnLoad]
    internal static class BindContextualPropertyMenu
    {
        static BindContextualPropertyMenu()
        {
            EditorApplication.contextualPropertyMenu -= OnMenu;
            EditorApplication.contextualPropertyMenu += OnMenu;
        }

        private static void OnMenu(GenericMenu menu, SerializedProperty property)
        {
            if (property.name != "_bindingValue") return;
            AddItem(menu);
        }

        private static void AddItem(GenericMenu menu)
        {
            menu.AddItem
            (
                content: new($"Paste 11"),
                on: false,
                func: () => { }
            );
        }
    }
}