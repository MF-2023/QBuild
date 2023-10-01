using System.Collections.Generic;

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
        }
        
        public IEnumerable<BlockPartScriptableObject> GetParts()
        {
            return _holdParts;
        }
        
        public BlockPartScriptableObject NextPart()
        {
            if (_holdParts.Count == 0)
            {
                _holdParts = new Queue<BlockPartScriptableObject>(_partList.GetParts());
            }
            _holdParts.TryDequeue(out var part);
            return part;
        }

        private PartListScriptableObject _partList;
        private Queue<BlockPartScriptableObject> _holdParts = new();
    }
}