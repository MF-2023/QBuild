using System;
using System.Collections.Generic;
using QBuild.Const;
using QBuild.Part.Operations;
using SherbetInspector.Core.Attributes;
using UnityEngine;

namespace QBuild.Part
{
    public enum EqualOperationType
    {
        Equal,
        NotEqual,
    }

    public class ProcessCalcParameter
    {
        public PartListScriptableObject PartListScriptableObject { get; private set; }
        public IEnumerable<GeneratePartInfo> Parts { get; private set; }
        public GeneratePartInfo CurrentPart { get; private set; }

        public ProcessCalcParameter(PartListScriptableObject partListScriptableObject,
            IEnumerable<GeneratePartInfo> parts, GeneratePartInfo currentPart)
        {
            PartListScriptableObject = partListScriptableObject;
            Parts = parts;
            CurrentPart = currentPart;
        }
    }

    [Serializable]
    public struct RandomOperator
    {
        [SerializeField] private BlockPartScriptableObject _target;
        [SerializeField] private EqualOperationType _equalOperationType;
        [SerializeReference, SubclassSelector] private IOperation _operation;

        public bool Process(ProcessCalcParameter parameter)
        {
            var canProcess = _equalOperationType switch
            {
                EqualOperationType.Equal => TargetEqual(parameter.CurrentPart),
                EqualOperationType.NotEqual => TargetNotEqual(parameter.CurrentPart),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _operation?.PreProcess(parameter, _target, canProcess);

            if (canProcess)
            {
                _operation?.Process(parameter, _target);
            }

            return canProcess;
        }

        private bool TargetEqual(GeneratePartInfo currentPart)
        {
            return _target == currentPart.Part;
        }

        private bool TargetNotEqual(GeneratePartInfo currentPart)
        {
            return _target != currentPart.Part;
        }
    }

    [CreateAssetMenu(fileName = "RandomOperator", menuName = EditorConst.ScriptablePrePath + "PartRandomPickObject",
        order = 1)]
    public class PartRandomPickObject : ScriptableObject
    {
        [SerializeField] private List<RandomOperator> _randomOperations = new();

        public void Processing(ProcessCalcParameter calcParameter)
        {
            _randomOperations.ForEach(x => x.Process(calcParameter));
        }
    }
}