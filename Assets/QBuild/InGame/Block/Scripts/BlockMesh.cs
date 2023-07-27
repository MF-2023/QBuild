using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    public static class BlockMesh
    {
        private static readonly Dictionary<BlockFace, Matrix4x4> FacePositions = new()
        {
            { BlockFace.Top, Matrix4x4.TRS(Vector3.up / 2, Quaternion.identity, Vector3.one) },
            { BlockFace.Left, Matrix4x4.TRS(Vector3.left / 2, Quaternion.Euler(0, 0, 90), Vector3.one) },
            { BlockFace.Front, Matrix4x4.TRS(Vector3.forward / 2, Quaternion.Euler(90, 0, 0), Vector3.one) },
            { BlockFace.Right, Matrix4x4.TRS(Vector3.right / 2, Quaternion.Euler(0, 0, -90), Vector3.one) },
            { BlockFace.Back, Matrix4x4.TRS(Vector3.back / 2, Quaternion.Euler(-90, 0, 0), Vector3.one) },
            { BlockFace.Bottom, Matrix4x4.TRS(Vector3.down / 2, Quaternion.Euler(180, 0, 0), Vector3.one) },
        };
        
        public static Mesh GenerateMesh(BlockGenerator generator)
        {
            var result = new Mesh();
            var combine = new CombineInstance[6];
            foreach (var dirFacePair in generator.GetFaces())
            {
                combine[(int)dirFacePair.dir].mesh = dirFacePair.face.GetFace().GetComponent<MeshFilter>().sharedMesh;
                combine[(int)dirFacePair.dir].transform = FacePositions[dirFacePair.dir];
            }

            result.CombineMeshes(combine);
            return result;
        }
    }
}