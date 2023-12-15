namespace QBuild.Part
{
    public abstract class BaseSlot
    {
        protected int _index;
        
        protected BlockPartScriptableObject _partObject;
        
        public void SetPart(BlockPartScriptableObject partObject)
        {
            _partObject = partObject;
        }

        public abstract BlockPartScriptableObject GetPart();
        
        public abstract BlockPartScriptableObject Use();
    }
}