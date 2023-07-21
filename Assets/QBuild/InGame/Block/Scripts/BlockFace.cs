using UnityEngine;

namespace QBuild
{
    public enum BlockFace : byte
    {
        Top,
        Bottom,
        Back,
        Left,
        Front,
        Right,
        None
        
        
    }
    
    public static class BlockFaceExtensions
    {
        public static Vector3Int ToBlockFaceVector(this BlockFace face)
        {
            return face switch
            {
                BlockFace.Top => Vector3Int.up,
                BlockFace.Bottom => Vector3Int.down,
                BlockFace.Left => Vector3Int.left,
                BlockFace.Right => Vector3Int.right,
                BlockFace.Front => Vector3Int.forward,
                BlockFace.Back => Vector3Int.back,
                _ => Vector3Int.zero
            };
        }
        
        public static BlockFace Opposite(this BlockFace face)
        {
            return face switch
            {
                BlockFace.Top => BlockFace.Bottom,
                BlockFace.Bottom => BlockFace.Top,
                BlockFace.Left => BlockFace.Right,
                BlockFace.Right => BlockFace.Left,
                BlockFace.Front => BlockFace.Back,
                BlockFace.Back => BlockFace.Front,
                _ => BlockFace.None
            };
        }
        
        public static BlockFace ToVectorBlockFace(this Vector3Int vector)
        {
            return vector switch
            {
                _ when vector == Vector3Int.up => BlockFace.Top,
                _ when vector == Vector3Int.down => BlockFace.Bottom,
                _ when vector == Vector3Int.left => BlockFace.Left,
                _ when vector == Vector3Int.right => BlockFace.Right,
                _ when vector == Vector3Int.forward => BlockFace.Front,
                _ when vector == Vector3Int.back => BlockFace.Back,
                _ => BlockFace.None
            };
        }
    }
}