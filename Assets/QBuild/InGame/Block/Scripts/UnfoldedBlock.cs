using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    public static class UnfoldedBlock
    {
        private static readonly Dictionary<BlockFace, Vector3> FacePositions = new()
        {
            { BlockFace.Top, new Vector3(0, 0, 1) },
            { BlockFace.Left, new Vector3(-1, 0, 0) },
            { BlockFace.Front, new Vector3(0, 0, 0) },
            { BlockFace.Right, new Vector3(1, 0, 0) },
            { BlockFace.Back, new Vector3(2, 0, 0) },
            { BlockFace.Bottom, new Vector3(0, 0, -1) },
        };

        public static Mesh GenerateMesh(BlockGenerator generator)
        {
            var result = new Mesh();
            var combine = new CombineInstance[6];
            foreach (var dirFacePair in generator.GetFaces())
            {
                combine[(int)dirFacePair.dir].mesh = dirFacePair.face.GetFace().GetComponent<MeshFilter>().sharedMesh;
                combine[(int)dirFacePair.dir].transform = Matrix4x4.Translate(FacePositions[dirFacePair.dir]);
            }

            result.CombineMeshes(combine);
            return result;
        }
    }
}