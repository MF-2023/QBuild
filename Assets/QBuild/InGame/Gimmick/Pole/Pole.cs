using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class Pole : MonoBehaviour
    {

        public struct WallInfo
        {
            public Wall wall;
            public Pole pairPole;
        }
    
        public List<WallInfo> wallInfos = new();
    }
}