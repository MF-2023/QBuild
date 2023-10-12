using System.Collections.Generic;
using System.Linq;

namespace QBuild.Part
{
    public class RandomPart
    {
        public RandomPart(PartListScriptableObject partList)
        {
            _parts = partList.GetParts().ToArray();
        }
        
        public BlockPartScriptableObject GetRandomPart()
        {
            return _parts[UnityEngine.Random.Range(0, _parts.Length)];
        }


        
        
        private readonly BlockPartScriptableObject[] _parts;
    }
}