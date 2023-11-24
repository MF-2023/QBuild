using System.Collections.Generic;
using UnityEditor;

namespace QBuild.LevelEditor
{
    public static class FileUtilities
    {
        private const string EditorPath = "Assets/QBuild/Editor/";
        private const string InGamePath = "Assets/QBuild/InGame/";

        public static T[] FindInGameAssetsOfType<T>(string path,string typeName = "") where T : UnityEngine.Object
        {
            var list = new List<T>();
            if (typeName == "")
            {
                typeName = typeof(T).ToString();
            }
            foreach (var guid in AssetDatabase.FindAssets("t:" + typeName, new[] { InGamePath + path }))
            {
                var o = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
                if (o != null)
                    list.Add(o);
            }

            return list.ToArray();
        }
    }
}