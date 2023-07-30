using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    [CreateAssetMenu(fileName = "PolyminoGeneratorList", menuName = "Tools/QBuild/PolyminoGeneratorList", order = 12)]
    public class PolyminoGeneratorList : ScriptableObject
    {
        [SerializeField] private List<PolyminoGenerator> _generators;

        public IReadOnlyList<PolyminoGenerator> Generators()
        { 
            return _generators;
        }
    }
}
