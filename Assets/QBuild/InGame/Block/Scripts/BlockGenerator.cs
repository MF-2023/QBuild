using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    [CreateAssetMenu(fileName = "BlockGenerator", menuName = "Tools/QBuild/BlockGenerator", order = 0)]
    public class BlockGenerator : ScriptableObject
    {
        public FaceScriptableObject top;
        public FaceScriptableObject center;
        public FaceScriptableObject right;
        public FaceScriptableObject back;
        public FaceScriptableObject left;
        public FaceScriptableObject bottom;
    }
}