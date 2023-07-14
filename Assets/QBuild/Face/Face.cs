using System;
using UnityEngine;

namespace QBuild
{
    
    [Serializable]
    public struct Face
    {
        [SerializeField] private string type;
        [SerializeField] private float maxLoaded;

        public Face(string type, float maxLoaded)
        {
            this.type = type;
            this.maxLoaded = maxLoaded;
        }
    }
}