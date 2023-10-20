using System;
using System.Linq;
using QBuild.Part.Event;
using SherbetInspector.Core.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.Part.Operations
{
    public enum ComparisonOperator
    {
        Equal,
        NotEqual,
        Less,
        LessEqual,
        Greater,
        GreaterEqual
    }

    [Serializable, TypeLabel("割合比較 条件イベント")]
    public class ComparisonProbabilityOperation : IOperation
    {
        [SerializeField] private ComparisonOperator _comparisonOperator;
        [SerializeField] private float _probability;
        [SerializeReference, SubclassSelector] private IPartListEvent _event = new ResetEvent();

        public void PreProcess(ProcessCalcParameter calcParameter, BlockPartScriptableObject target, bool isCurrentPart)
        {
            
        }

        public bool Process(ProcessCalcParameter calcParameter, BlockPartScriptableObject target)
        {
            if (Comparison(calcParameter, target))
            {
                _event.Execute(calcParameter, target);
            }

            return true;
        }

        private bool Comparison(ProcessCalcParameter calcParameter, BlockPartScriptableObject target)
        {
            bool result = false;
            var part = calcParameter.Parts.SingleOrDefault(x => x.Part == target);
            result = _comparisonOperator switch
            {
                ComparisonOperator.Equal => part.Probability == _probability,
                ComparisonOperator.NotEqual => part.Probability != _probability,
                ComparisonOperator.Less => part.Probability < _probability,
                ComparisonOperator.LessEqual => part.Probability <= _probability,
                ComparisonOperator.Greater => part.Probability > _probability,
                ComparisonOperator.GreaterEqual => part.Probability >= _probability,
                _ => false
            };

            return result;
        }
    }


    [Serializable, TypeLabel("連続回数比較 条件イベント")]
    public class ComparisonTimesOperation : IOperation
    {
        [SerializeField] private ComparisonOperator _comparisonOperator;
        [SerializeField] private int _currentTimes;
        private int _times;
        [SerializeReference, SubclassSelector] private IPartListEvent _event = new ResetEvent();

        public void PreProcess(ProcessCalcParameter calcParameter, BlockPartScriptableObject target, bool isCurrentPart)
        {
            if (!isCurrentPart)
            {
                _times = 0;
            }
        }

        public bool Process(ProcessCalcParameter calcParameter, BlockPartScriptableObject target)
        {
            _times++;
            if (Comparison(calcParameter, target))
            {
                Debug.Log("連続回数条件イベント発火");
                _event.Execute(calcParameter, target);
            }

            return true;
        }

        private bool Comparison(ProcessCalcParameter calcParameter, BlockPartScriptableObject target)
        {
            bool result = false;
            var part = calcParameter.Parts.SingleOrDefault(x => x.Part == target);
            result = _comparisonOperator switch
            {
                ComparisonOperator.Equal => _times == _currentTimes,
                ComparisonOperator.NotEqual => _times != _currentTimes,
                ComparisonOperator.Less => _times < _currentTimes,
                ComparisonOperator.LessEqual => _times <= _currentTimes,
                ComparisonOperator.Greater => _times > _currentTimes,
                ComparisonOperator.GreaterEqual => _times >= _currentTimes,
                _ => false
            };

            return result;
        }
    }
}