using UnityEngine;

namespace QBuild.Part.PartScriptableObject
{
    public abstract class BasePartSpawnConfiguratorObject : ScriptableObject
    {
        public abstract BlockPartScriptableObject GetPartObject(int hint = -1);
    }
}