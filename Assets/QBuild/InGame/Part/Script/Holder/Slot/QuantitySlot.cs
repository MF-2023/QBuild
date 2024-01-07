using System;

namespace QBuild.Part
{
    public class QuantitySlot : BaseSlot
    {
        public override event Action OnDisable;

        private int _quantity;
        public int Quantity => _quantity;
        
        public QuantitySlot(BlockPartScriptableObject partObject,int index, int quantity)
        {
            _partObject = partObject;
            _index = index;
            _quantity = quantity;
        }


        public override BlockPartScriptableObject GetPart()
        {
            if (_quantity <= 0)
                return null;
            return _partObject;
        }

        public override BlockPartScriptableObject Use()
        {
            if (_quantity <= 0)
                return null;
            _quantity--;
            if(_quantity <= 0)
            {
                _disable = true;
                OnDisable?.Invoke();
            }
            return _partObject;
        }
    }
}