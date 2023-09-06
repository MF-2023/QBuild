using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    public struct BoundBlockState
    {
        public BoundBlockState(Vector3Int min, Vector3Int max, BlockState[] blockStates)
        {
            _blockStates = blockStates;
            this.Min = min;
            this.Max = max;
            var size = max - min + Vector3Int.one;
            if (blockStates.Length == size.x * size.y * size.z)
            {
                IsValidate = true;
            }
            else
            {
                IsValidate = false;
            }
        }

        public BlockState GetBlockState(Vector3Int pos)
        {
            if (IsValidate == false) return BlockState.NullBlock;

            var size = Size;
            var index = (pos.x - Min.x) + (pos.y - Min.y) * size.x + (pos.z - Min.z) * size.x * size.y;
            if (pos.x < Min.x || pos.x > Max.x || pos.y < Min.y || pos.y > Max.y || pos.z < Min.z || pos.z > Max.z)
            {
                return BlockState.NullBlock;
            }

            return _blockStates[index];
        }

        public Vector3Int Size => Max - Min + Vector3Int.one;

        public bool IsValidate { get; private set; }

        public Vector3Int Min { get; private set; }
        public Vector3Int Max { get; private set; }

        private BlockState[] _blockStates;
    }
}