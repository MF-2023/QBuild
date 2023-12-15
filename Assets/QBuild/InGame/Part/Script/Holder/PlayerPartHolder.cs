using System;
using System.Collections.Generic;
using QBuild.Part.PartScriptableObject;
using VContainer;

namespace QBuild.Part
{
    public class PlayerPartHolder : IPartsHoldable
    {
        public event HolderUsedEventHandler OnUsePart;
        public event HolderSelectChangedEventHandler OnChangedSelect;


        private BasePartSpawnConfiguratorObject _basePartSpawnConfiguratorObject;
        private List<BaseSlot> _slots = new();
        public IEnumerable<BaseSlot> Slots => _slots;

        private int _currentPartIndex = 0;
        private int _prevPartIndex = 0;

        public PlayerPartHolder(BasePartSpawnConfiguratorObject basePartSpawnConfiguratorObject, int holderSize)
        {
            _basePartSpawnConfiguratorObject = basePartSpawnConfiguratorObject;
            //TODO: partを設定する
        }

        public void Next()
        {
            _prevPartIndex = _currentPartIndex;
            _currentPartIndex++;
            if (_currentPartIndex >= _slots.Count)
            {
                _currentPartIndex = 0;
            }

            ChangedSelect();
        }

        public void Prev()
        {
            _prevPartIndex = _currentPartIndex;
            _currentPartIndex--;
            if (_currentPartIndex < 0)
            {
                _currentPartIndex = _slots.Count - 1;
            }

            ChangedSelect();
        }

        private void ChangedSelect()
        {
            OnChangedSelect?.Invoke(this,
                new HolderSelectChangeEventArgs(_slots[_currentPartIndex].GetPart(), _slots[_currentPartIndex],
                    _currentPartIndex, _prevPartIndex));
        }

        public BlockPartScriptableObject GetCurrentPart()
        {
            return _slots[_currentPartIndex].GetPart();
        }

        public void Use()
        {
            var part = _slots[_currentPartIndex].Use();
            OnUsePart?.Invoke(this,
                new HolderUseEventArgs(_slots[_currentPartIndex].GetPart(), _slots[_currentPartIndex],
                    _currentPartIndex));
        }
    }
}