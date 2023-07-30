using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    [CreateAssetMenu(fileName = "BlockGenerator", menuName = "Tools/QBuild/BlockGenerator", order = 11)]
    public class BlockGenerator : ScriptableObject
    {
        public FaceScriptableObject top;
        public FaceScriptableObject center;
        public FaceScriptableObject right;
        public FaceScriptableObject back;
        public FaceScriptableObject left;
        public FaceScriptableObject bottom;

        public struct NameFacePair
        {
            public BlockFace dir { get; }
            public FaceScriptableObject face { get; }

            public NameFacePair(BlockFace dir, FaceScriptableObject face)
            {
                this.dir = dir;
                this.face = face;
            }
        }
        public IEnumerable<NameFacePair> GetFaces()
        {
            return new NameFacePair[]
            {
                new(BlockFace.Top, top), new(BlockFace.Front, center), new(BlockFace.Right, right),
                new(BlockFace.Back, back), new(BlockFace.Left, left), new(BlockFace.Bottom, bottom)
            };
        }
        
    }
}