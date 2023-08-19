using UnityEngine;

namespace QBuild.Mino
{
    public interface IMinoFactory
    {
        public Polyomino CreateMino(MinoType minoType, Vector3Int origin, Transform parent);
    }
}