using System.Collections.Generic;

namespace QBuild.Part
{
    public interface IPartsHoldable
    {
        event System.Action<IEnumerable<BlockPartScriptableObject>> OnChangeParts;
    }
}