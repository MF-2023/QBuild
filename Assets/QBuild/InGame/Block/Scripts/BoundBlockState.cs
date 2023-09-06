using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    public struct BoundBlockState
    {
        public BoundBlockState(Vector3Int min, Vector3Int max, BlockState[] blockStates)
        {
            _blockStates = blockStates;
            this.min = min;
            this.max = max;
            var size = max - min + Vector3Int.one;
            if (blockStates.Length == size.x * size.y * size.z)
            {
                isValidate = true;
            }
            else
            {
                isValidate = false;
            }
        }

        public BlockState GetBlockState(Vector3Int pos)
        {
            if(isValidate == false) return BlockState.NullBlock;
            
            var size = Size;
            var index = (pos.x - min.x) + (pos.y - min.y) * size.x + (pos.z - min.z) * size.x * size.y;
            if (pos.x < min.x || pos.x > max.x || pos.y < min.y || pos.y > max.y || pos.z < min.z || pos.z > max.z)
            {
                return BlockState.NullBlock;
            }

            return _blockStates[index];
        }

        public Vector3Int Size => max - min + Vector3Int.one;

        public bool isValidate;

        public Vector3Int min;
        public Vector3Int max;

        private BlockState[] _blockStates;
    }
}