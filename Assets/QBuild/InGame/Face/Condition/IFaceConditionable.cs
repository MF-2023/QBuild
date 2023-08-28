using UnityEngine;

namespace QBuild
{
    public abstract class FaceConditionable : ScriptableObject
    {
        public abstract bool IsExclude(FaceScriptableObject face1, FaceScriptableObject face2);
    }
}