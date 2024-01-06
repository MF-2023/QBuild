using System;

namespace QBuild.Part
{
    public abstract class BaseSlot
    {
        public abstract event Action OnDisable;
        
        protected bool _disable;
        public bool Disable => _disable;
        
        protected int _index;
        public int Index => _index;
        
        protected BlockPartScriptableObject _partObject;
        
        public void SetPart(BlockPartScriptableObject partObject)
        {
            _partObject = partObject;
        }

        public abstract BlockPartScriptableObject GetPart();
        
        public abstract BlockPartScriptableObject Use();
    }
}