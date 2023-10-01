using System.Collections.Generic;

namespace QBuild.Part
{
    public interface IPartsHoldable
    {
        IEnumerable<BlockPartScriptableObject> GetParts();
    }
}