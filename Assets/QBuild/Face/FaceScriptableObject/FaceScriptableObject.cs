using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild
{
    
    [CreateAssetMenu(fileName = "FaceScriptable", menuName = "Tools/QBuild/Face", order = 0)]
    public class FaceScriptableObject : ScriptableObject
    {
        [SerializeField] private string type;
        [SerializeField] private GameObject facePrefab;
        [FormerlySerializedAs("maxLoaded")] [SerializeField] private float stability; 

        public GameObject GetFace()
        {
            return facePrefab;
        }

        public Face MakeFace()
        {
            return new Face(type,stability);
        }
    }
}