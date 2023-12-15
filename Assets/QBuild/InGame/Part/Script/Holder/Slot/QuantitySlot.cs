namespace QBuild.Part
{
    public class QuantitySlot : BaseSlot
    {
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
            return _partObject;
        }
    }
}