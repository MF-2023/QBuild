using QBuild.Const;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild
{
    [CreateAssetMenu(fileName = "FaceScriptable", menuName = EditorConst.ScriptablePrePath + "Face",
        order = EditorConst.OtherOrder)]
    public class FaceScriptableObject : ScriptableObject
    {
        [SerializeField] private string type;
        [SerializeField] private GameObject facePrefab;

        public GameObject GetFace()
        {
            return facePrefab;
        }

        public Face MakeFace()
        {
            return new Face(this);
        }
    }
}