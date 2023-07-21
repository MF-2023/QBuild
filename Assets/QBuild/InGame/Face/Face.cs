using System;
using JetBrains.Annotations;
using UnityEngine;

namespace QBuild
{
    
    [Serializable]
    public struct Face
    {
        [SerializeField] private FaceScriptableObject type;
        
        public Block glueBlock { private set; get; }
        

        public Face(FaceScriptableObject type)
        {
            this.type = type;
            glueBlock = null;
        }
        public FaceScriptableObject GetFaceType()
        {
            return type;
        }
        public void SetGlueBlock(Block block)
        {
            glueBlock = block;
        }
    }
}