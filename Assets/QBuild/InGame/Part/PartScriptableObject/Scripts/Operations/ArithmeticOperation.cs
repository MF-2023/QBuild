using System;
using System.Collections.Generic;
using System.Linq;
using SherbetInspector.Core.Attributes;
using UnityEngine;

namespace QBuild.Part.Operations
{
    public interface IOperation
    {
        public void PreProcess(ProcessCalcParameter calcParameter,
            BlockPartScriptableObject target, bool isCurrentPart);

        public bool Process(ProcessCalcParameter calcParameter,
            BlockPartScriptableObject target);
    }

    public enum ArithmeticOperationType
    {
        Add,
        Sub,
        Mul,
        Div,
    }

    [Serializable, TypeLabel("算術演算")]
    public class ArithmeticOperation : IOperation
    {
        [SerializeField] private ArithmeticOperationType _arithmeticOperationType;
        [SerializeField] private float _arithmeticValue;
        

        public void PreProcess(ProcessCalcParameter calcParameter, BlockPartScriptableObject target, bool isCurrentPart)
        {
            
        }

        public bool Process(ProcessCalcParameter calcParameter,
            BlockPartScriptableObject target)
        {
            var part = calcParameter.Parts.SingleOrDefault(x => x.Part == target);
            if (part != null)
                Arithmetic(part);
            return true;
        }

        private void Arithmetic(GeneratePartInfo currentPart)
        {
            switch (_arithmeticOperationType)
            {
                case ArithmeticOperationType.Add:
                    currentPart.Probability += _arithmeticValue;
                    break;
                case ArithmeticOperationType.Sub:
                    currentPart.Probability -= _arithmeticValue;
                    break;
                case ArithmeticOperationType.Mul:
                    currentPart.Probability *= _arithmeticValue;
                    break;
                case ArithmeticOperationType.Div:
                    currentPart.Probability /= _arithmeticValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}