using System;
using UnityEngine;

namespace QBuild.Mino
{
    public interface IMinoFactory
    {
        public event Action<Polyomino> OnMinoCreated;
        public Polyomino CreateMino(MinoType minoType, Vector3Int origin, Transform parent);
    }
}