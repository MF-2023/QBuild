using QBuild.Part.PartScriptableObject;
using QBuild.Part.PartScriptableObject.ConfiguratorObject;

namespace QBuild.Part
{
    public interface ISlotFactory
    {
        BaseSlot CreateSlot(BasePartSpawnConfiguratorObject configuratorObject, int index);
    }
    
    public class QuantitySlotFactory : ISlotFactory
    {
        public BaseSlot CreateSlot(BasePartSpawnConfiguratorObject configuratorObject, int index)
        {
            return CreateSlotInternal(configuratorObject as QuantitySpawnConfiguratorObject, index);
        }
        
        private QuantitySlot CreateSlotInternal(QuantitySpawnConfiguratorObject configuratorObject, int index)
        {
            var partObject = configuratorObject.GetPartObject(index);
            var quantity = configuratorObject.GetQuantity(index);
            return new QuantitySlot(partObject, index, quantity);
        }
    }
}