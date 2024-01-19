using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class StageEditorPole : MonoBehaviour
{

    public struct WallInfo
    {
        public StageEditorWall wall;
        public StageEditorPole pairPole;
    }
    
     public List<WallInfo> wallInfos = new();
}