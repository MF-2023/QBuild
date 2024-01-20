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

        public event HolderSlotsUpdatedEventHandler OnSlotsUpdated;

        public IEnumerable<BaseSlot> Slots => _slots;
        public int CurrentPartIndex => _currentPartIndex;

        private BasePartSpawnConfiguratorObject _basePartSpawnConfiguratorObject;
        private ISlotFactory _slotFactory;
        private List<BaseSlot> _slots = new();

        private int _holderSize = 0;
        private int _currentPartIndex = 0;
        private int _prevPartIndex = 0;

        public PlayerPartHolder(BasePartSpawnConfiguratorObject basePartSpawnConfiguratorObject, int holderSize)
        {
            _basePartSpawnConfiguratorObject = basePartSpawnConfiguratorObject;
            _slotFactory = new QuantitySlotFactory();
            _holderSize = holderSize;
        }

        public void Initialize()
        {
            for (var i = 0; i < _holderSize; i++)
            {
                var slot = _slotFactory.CreateSlot(_basePartSpawnConfiguratorObject, i);
                _slots.Add(slot);
            }
            
            OnSlotsUpdated?.Invoke(this, new HolderSlotsUpdateEventArgs(_slots));
        }


        public void Next()
        {
            _prevPartIndex = _currentPartIndex;
            do
            {
                _currentPartIndex++;
                if (_currentPartIndex >= _slots.Count)
                {
                    _currentPartIndex = 0;
                }
            } while (_slots[_currentPartIndex].Disable && _currentPartIndex != _prevPartIndex);

            ChangedSelect();
        }

        public void Prev()
        {
            _prevPartIndex = _currentPartIndex;
            do
            {
                _currentPartIndex--;
                if (_currentPartIndex < 0)
                {
                    _currentPartIndex = _slots.Count - 1;
                }
            } while (_slots[_currentPartIndex].Disable && _currentPartIndex != _prevPartIndex);

            ChangedSelect();
        }


        public void Use()
        {
            var slot = GetCurrentSlot();
            var part = slot.Use();
            OnUsePart?.Invoke(this,
                new HolderUseEventArgs(slot.GetPart(), slot, _currentPartIndex));

            if (slot.Disable)
            {
                _slots.RemoveAt(CurrentPartIndex);
                OnSlotsUpdated?.Invoke(this, new HolderSlotsUpdateEventArgs(_slots));
            }
        }

        public BlockPartScriptableObject GetCurrentPart()
        {
            if (_currentPartIndex >= _slots.Count)
            {
                return null;
            }
            return _slots[_currentPartIndex].GetPart();
        }

        private BaseSlot GetCurrentSlot()
        {
            return _slots[_currentPartIndex];
        }

        private void ChangedSelect()
        {
            var slot = GetCurrentSlot();
            OnChangedSelect?.Invoke(this,
                new HolderSelectChangeEventArgs(slot.GetPart(), slot,
                    _currentPartIndex, _prevPartIndex));
        }
    }
}