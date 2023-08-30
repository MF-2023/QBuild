using UnityEngine;

namespace QBuild
{
    public interface IBlockParentObject
    {
        GameObject GetParentObject();
        Transform Transform => GetParentObject().transform;
    }
    public class BlockParentObject : MonoBehaviour,IBlockParentObject
    {

        public GameObject GetParentObject()
        {
            return gameObject;
        }
    }
}