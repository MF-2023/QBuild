using System.IO;
using UnityEditor;

namespace QBuild.Part.Editor
{
    [CustomEditor(typeof(PartView))]
    public class PartViewEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var partView = (PartView)target;
            if (partView == null)
            {
                EditorUtility.SetDirty(partView);
            }
        }
        [MenuItem("CONTEXT/PartView/CreateScriptableObject")]
        static void CreateScriptableObject(MenuCommand command)
        {
            var obj = CreateInstance<BlockPartScriptableObject>();
            obj.SetPartPrefab(command.context as PartView);
            var fileName = $"{command.context.name}.asset";
            var path = "Assets/QBuild/InGame/Part/PartScriptableObject";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(obj, Path.Combine(path, fileName));
        }
    }
}