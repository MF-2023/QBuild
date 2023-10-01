using UnityEditor;

namespace QBuild.Part.Editor
{
    [CustomEditor(typeof(PartView))]
    public class PartViewEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            PartView myComponent = (PartView)target;
            if (myComponent == null)
            {

                EditorUtility.SetDirty(myComponent);
            }
        }
    }
}