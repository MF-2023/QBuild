using System;
using System.Collections.Generic;
using VContainer;

namespace QBuild.Part
{
    /// <summary>
    /// 設置するパーツを保持するクラス
    /// </summary>
    public class NextPartHolder : IPartsHoldable
    {
        public NextPartHolder(PartListScriptableObject partListScriptableObject)
        {
            _partList = partListScriptableObject;
            _holdParts.Enqueue(_partList.GetRandomPart());
        }

        public event Action<IEnumerable<BlockPartScriptableObject>> OnChangeParts;

        public IEnumerable<BlockPartScriptableObject> GetParts()
        {
            return _holdParts;
        }

        public BlockPartScriptableObject NextPart()
        {
            _holdParts.TryDequeue(out var part);
            _holdParts.Enqueue(_partList.GetRandomPart());

            OnChangeParts?.Invoke(_holdParts);
            return part;
        }
        
        public BlockPartScriptableObject CurrentPart()
        {
            return _holdParts.Peek();
        }

        private PartListScriptableObject _partList;
        private Queue<BlockPartScriptableObject> _holdParts = new();
    }
}