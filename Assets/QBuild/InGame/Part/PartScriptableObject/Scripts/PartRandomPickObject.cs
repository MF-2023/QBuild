using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Const;
using QBuild.Part.Operations;
using SherbetInspector.Core.Attributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QBuild.Part
{
    public enum EqualOperationType
    {
        Equal,
        NotEqual,
    }

    public class PickCalcParameter
    {
        public PartListScriptableObject PartListScriptableObject { get; private set; }
        public IEnumerable<GeneratePartInfo> Parts { get; private set; }

        public PickCalcParameter(PartListScriptableObject partListScriptableObject,
            IEnumerable<GeneratePartInfo> parts)
        {
            PartListScriptableObject = partListScriptableObject;
            Parts = parts;
        }
    }

    public class ProcessCalcParameter
    {
        public PartListScriptableObject PartListScriptableObject { get; set; }
        public IEnumerable<GeneratePartInfo> Parts { get; set; }
        public GeneratePartInfo CurrentPart { get; set; }

        public ProcessCalcParameter(PickCalcParameter pickCalcParameter, GeneratePartInfo currentPart)
        {
            PartListScriptableObject = pickCalcParameter.PartListScriptableObject;
            Parts = pickCalcParameter.Parts;
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
    public class PartRandomPickObject : PartPickObject
    {
        [SerializeField] private List<RandomOperator> _randomOperations = new();

        public override BlockPartScriptableObject Processing(IEnumerable<GeneratePartInfo> partInfos,
            PickCalcParameter calcParameter)
        {
            var generatePartInfos = partInfos as GeneratePartInfo[] ?? partInfos.ToArray();
            var total = generatePartInfos.Sum(x => x.Probability);
            var random = Random.Range(0, total);
            var current = 0f;
            foreach (var part in generatePartInfos)
            {
                current += part.Probability;
                if (!(random < current)) continue;
                _randomOperations.ForEach(x => x.Process(new ProcessCalcParameter(calcParameter, part)));
                return part.Part;
            }

            Debug.LogError("GetRandomPart() failed.");
            return null;
        }
    }
}