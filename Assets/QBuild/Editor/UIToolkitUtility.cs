using UnityEditor;
using UnityEngine.UIElements;

namespace QBuild
{
    public static class UIToolkitUtility
    {
        private const string DirectoryPath = "Assets/QBuild/Editor/";

        public static VisualTreeAsset GetVisualTree(string path)
        {
            var fullPath = DirectoryPath + path + ".uxml";
            return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(fullPath);
        }
    }
}