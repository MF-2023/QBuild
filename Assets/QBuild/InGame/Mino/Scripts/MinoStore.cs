using System.Collections.Generic;

namespace QBuild.Mino
{
    /// <summary>
    /// ミノを管理するクラス
    /// </summary>
    public class MinoStore
    {
        public void AddMino(MinoKey key, Polyomino mino)
        {
            _minoDictionary.Add(key, mino);
        }
        
        public bool TryGetMino(MinoKey key, out Polyomino mino)
        {
            mino = null;
            if (!_minoDictionary.ContainsKey(key)) return false;

            mino = _minoDictionary[key];
            return true;
        }

        public bool RemoveMino(MinoKey key)
        {
            return _minoDictionary.Remove(key);
        }

        public void Clear()
        {
            _minoDictionary.Clear();
        }

        public int CreateKey()
        {
            _createCount++;
            return _createCount;
        }
        public int Count => _minoDictionary.Count;
        private int _createCount = 0;
        
        private readonly Dictionary<MinoKey, Polyomino> _minoDictionary = new();
    }
}