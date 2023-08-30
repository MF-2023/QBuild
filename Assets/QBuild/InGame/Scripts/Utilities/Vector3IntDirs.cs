using UnityEngine;

namespace QBuild
{
    public static class Vector3IntDirs
    {
        public static readonly Vector3Int[] HorizontalDirections =
        {
            new(1, 0, 0),
            new(-1, 0, 0),
            new(0, 0, 1),
            new(0, 0, -1)
        };
        
        public static readonly Vector3Int[] AllDirections =
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.forward,
            Vector3Int.back
        };
    }
}