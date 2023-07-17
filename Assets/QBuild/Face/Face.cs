using System;
using JetBrains.Annotations;
using UnityEngine;

namespace QBuild
{
    
    [Serializable]
    public struct Face
    {
        [SerializeField] private string type;
        [SerializeField] private float maxLoaded;
        
        public Block glueBlock { private set; get; }
        

        public Face(string type, float maxLoaded)
        {
            this.type = type;
            this.maxLoaded = maxLoaded;
            glueBlock = null;
        }
        
        public void SetGlueBlock(Block block)
        {
            glueBlock = block;
        }
    }
}