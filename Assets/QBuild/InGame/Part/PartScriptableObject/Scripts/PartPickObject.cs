using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Part
{
    public abstract class PartPickObject : ScriptableObject
    {
        public abstract BlockPartScriptableObject Processing(IEnumerable<GeneratePartInfo> partInfos,PickCalcParameter calcParameter);
    }
}