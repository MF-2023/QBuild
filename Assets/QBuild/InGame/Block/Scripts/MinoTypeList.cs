using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Const;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild
{
    [CreateAssetMenu(fileName = "PolyminoGeneratorList",
        menuName = EditorConst.ScriptablePrePath + "PolyminoGeneratorList", order = EditorConst.OtherOrder)]
    public class MinoTypeList : ScriptableObject
    {
        [Serializable]
        private class MinoTypeElement
        {
            [SerializeField] private int _expectedValue;
            [SerializeField] private MinoType _minoType;

            public MinoType GetMinoType()
            {
                return _minoType;
            }

            public int GetExpectedValue()
            {
                return _expectedValue;
            }
        }

        [SerializeField] private List<MinoTypeElement> _types;

        public MinoType NextGenerator()
        {
            var list = _types.Select(element => element.GetExpectedValue());

            var index = GetRandomMino(list);

            var generator = _types[index].GetMinoType();

            if (generator == null)
            {
                Debug.LogError($"{index + 1}番目のGeneratorが割り当てられていません", this);
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
            return _types.Count;
        }
    }
}