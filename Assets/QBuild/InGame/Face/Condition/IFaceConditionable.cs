using UnityEngine;

namespace QBuild.Condition
{
    public abstract class FaceConditionable : ScriptableObject
    {
        public abstract bool IsExclude(FaceScriptableObject face1, FaceScriptableObject face2);
    }
}