using System;
using UnityEngine;

namespace QBuild.Face
{
    
    [Serializable]
    public struct Face
    {
        [SerializeField] private FaceScriptableObject faceObject;
        [SerializeField] private int loaded;

    }
}