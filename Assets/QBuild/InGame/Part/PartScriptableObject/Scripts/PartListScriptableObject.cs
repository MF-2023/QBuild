using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Const;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QBuild.Part
{
    [Serializable]
    public class GeneratePartInfo : IEquatable<GeneratePartInfo>
    {
        [SerializeField] private BlockPartScriptableObject _part;
        [SerializeField] private float _probability;

        public BlockPartScriptableObject Part => _part;

        public float Probability
        {
            get => _probability;
            set => _probability = value;
        }

        public GeneratePartInfo(BlockPartScriptableObject part, float probability)
        {
            _part = part;
            _probability = probability;
        }


        public bool Equals(GeneratePartInfo other)
        {
            return Equals(_part, other._part);
        }

        public override int GetHashCode()
        {
            return (_part != null ? _part.GetHashCode() : 0);
        }
    }

    [Serializable]
    public class PartList
    {
        [SerializeField] private List<GeneratePartInfo> _parts = new();

        private Queue<BlockPartScriptableObject> _reservationParts = new();
        private PartListScriptableObject _owner;

        public void SetPartList(IEnumerable<GeneratePartInfo> parts, PartListScriptableObject owner)
        {
            _owner = owner;
            foreach (var generatePartInfo in parts)
            {
                _parts.Add(new GeneratePartInfo(generatePartInfo.Part, generatePartInfo.Probability));
            }
        }

        public void SetOwner(PartListScriptableObject owner)
        {
            _owner = owner;
        }
        
        public void AddReservationPart(BlockPartScriptableObject part)
        {
            _reservationParts.Enqueue(part);
        }

        public IEnumerable<GeneratePartInfo> GeneratePartInfos => _parts;

        public IEnumerable<BlockPartScriptableObject> GetParts()
        {
            return _parts.Select(x => x.Part);
        }

        public BlockPartScriptableObject GetRandomPart(PartRandomPickObject randomPickObject)
        {
            if (_reservationParts.Count != 0)
            {
                // 予約パーツがある場合は予約パーツを返す
                return _reservationParts.Dequeue();
            }
            var total = _parts.Sum(x => x.Probability);
            var random = Random.Range(0, total);
            var current = 0f;
            foreach (var part in _parts)
            {
                current += part.Probability;
                if (random < current)
                {
                    randomPickObject.Processing(new ProcessCalcParameter(_owner, _parts, part));
                    return part.Part;
                }
            }

            Debug.LogError("GetRandomPart() failed.");
            return _parts[0].Part;
        }
    }


    /// <summary>
    /// ゲームで利用するパーツのリスト
    /// </summary>
    [CreateAssetMenu(fileName = "PartList", menuName = EditorConst.VariablePrePath + "PartList",
        order = EditorConst.OtherOrder)]
    public class PartListScriptableObject : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private PartList _parts;
        [SerializeField] private PartRandomPickObject _partRandomPickObject;
        [SerializeField] private PartList _runtimeParts;
        
        public IEnumerable<BlockPartScriptableObject> GetParts()
        {
            return _parts.GetParts();
        }

        public BlockPartScriptableObject GetRandomPart()
        {
            var result = _runtimeParts.GetRandomPart(_partRandomPickObject);
            return result;
        }
        
        public void ResetProbability(BlockPartScriptableObject so)
        {
            var runtimeInfo = _runtimeParts.GeneratePartInfos.SingleOrDefault(x => x.Part == so);
            var info = _parts.GeneratePartInfos.SingleOrDefault(x => x.Part == so);
            if (runtimeInfo != null)
            {
                runtimeInfo.Probability = info.Probability;
            }
        }
        public void AddReservationPart(BlockPartScriptableObject part)
        {
            _runtimeParts.AddReservationPart(part);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            _parts.SetOwner(this);
            _runtimeParts = new PartList();
            _runtimeParts.SetPartList(_parts.GeneratePartInfos, this);
        }
    }
}