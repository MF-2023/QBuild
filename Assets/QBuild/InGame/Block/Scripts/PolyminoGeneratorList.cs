using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QBuild
{
    [CreateAssetMenu(fileName = "PolyminoGeneratorList", menuName = "Tools/QBuild/PolyminoGeneratorList", order = 12)]
    public class PolyminoGeneratorList : ScriptableObject
    {
        [Serializable]
        private class MinoGeneratorElement
        {
            [SerializeField] private int _expectedValue;
            [SerializeField] private PolyminoGenerator _generator;

            public PolyminoGenerator GetGenerator()
            {
                return _generator;
            }

            public int GetExpectedValue()
            {
                return _expectedValue;
            }
        }
        
        [SerializeField] private List<MinoGeneratorElement> _generators;

        public PolyminoGenerator NextGenerator()
        {
            var list = _generators.Select(element => element.GetExpectedValue());

            var index = GetRandomMino(list);

            var generator =  _generators[index].GetGenerator();

            if (generator == null)
            {
                Debug.LogError($"{index + 1}番目のGeneratorが割り当てられていません",this);
                return null;
            }
            
            Debug.Log($"generate mino {generator.name}");
            
            return generator;
        }
        
        private int GetRandomMino(IEnumerable<int> ratioList)
        {
            var enumerable = ratioList as int[] ?? ratioList.ToArray();
            
            var total = enumerable.Sum();

            //calculation random value
            var randomPoint = UnityEngine.Random.value * total;

            for (var i = 0; i < enumerable.Count(); i++)
            {
                if (randomPoint < enumerable[i]) return i;

                randomPoint -= enumerable[i];
            }

            return enumerable.Count() - 1;
        }


        [Obsolete("実装に依存してしまうため、使わないこと")]
        public int GetCount()
        {
            return _generators.Count;
        }
    }
}
