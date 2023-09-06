using System;
using System.Linq;
using QBuild.Mino;
using SherbetInspector.Core.Attributes;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace QBuild
{
    [Serializable]
    public struct Faces
    {
        public Face top;
        public Face center;
        public Face right;
        public Face back;
        public Face left;
        public Face bottom;
    }


    public struct BlockState : IEquatable<BlockState>
    {
        public bool isValidate;
        public Vector3Int position;

        public BlockState(Vector3Int position, bool isValidate = true)
        {
            this.position = position;
            this.isValidate = isValidate;
        }

        public static BlockState NullBlock => new(Vector3Int.zero, false);

        public bool Equals(BlockState other)
        {
            return (isValidate == false && other.isValidate == false) ||
                   (isValidate == other.isValidate && position.Equals(other.position));
        }

        public override bool Equals(object obj)
        {
            return obj is BlockState other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(isValidate, position);
        }
    }

    public class Block : MonoBehaviour
    {
        [SerializeField] private BlockType blockScriptableObjects;

        [SerializeField] private Faces faces;


        [SerializeField] private Vector3Int _gridPosition;

        [SerializeField] private float _stabilityGlue = 0;
        [SerializeField] private float _stability = 0;
        [SerializeField] private float _mass = 1;
        private BlockState _blockState = BlockState.NullBlock;

        [Inject]
        private void Inject(BlockService blockService)
        {
            Debug.Log("Inject Block");
            _blockService = blockService;
        }

        public void GenerateBlock(BlockType type, Vector3Int pos)
        {
            _ownerMinoKey = MinoKey.NullMino;

            blockScriptableObjects = type;
            _gridPosition = pos;
            _blockState = new BlockState(pos);
            GenerateBlock();
        }

        public MinoKey GetMinoKey()
        {
            return _ownerMinoKey;
        }

        public void SetMinoKey(MinoKey key)
        {
            _ownerMinoKey = key;
        }

        public bool CanMove(Vector3Int move)
        {
            return _blockService.CanPlace(_gridPosition + move);
        }

        public void MoveNext(Vector3Int move)
        {
            var before = _gridPosition;
            _gridPosition += move;
            _blockState.position = _gridPosition;
            transform.localPosition = _gridPosition;
            _blockService.UpdateBlock(this, before);

            name = $"Block_{_gridPosition}";
        }
        
        public void SetPosition(Vector3Int position)
        {
            var before = _gridPosition;
            _gridPosition = position;
            _blockState.position = _gridPosition;
            transform.localPosition = _gridPosition;
            _blockService.UpdateBlock(this, before);

            name = $"Block_{_gridPosition}";
        }

        public Vector3Int GetGridPosition()
        {
            return _gridPosition;
        }

        public BlockState GetBlockState()
        {
            return _blockState;
        }

        public void OnFall()
        {
            _isFalling = true;
        }

        public void OnBlockPlaced(float stabilityNext = -1)
        {
            _isFalling = false;

            if (stabilityNext < 0) stabilityNext = CalcStability();

            _stability = stabilityNext;
            Debug.Log("Place");
        }

        private float CalcStability()
        {
            float supportHorizontalStability = 0;

            foreach (var t in Vector3IntDirs.HorizontalDirections)
            {
                if (_blockService.TryGetBlock(_gridPosition + t, out var block))
                {
                    supportHorizontalStability = Math.Max(block.GetStability(), supportHorizontalStability);
                }
            }

            supportHorizontalStability--;

            float stabilityUp = 0;
            float stabilityDown = 0;

            if (_blockService.TryGetBlock(_gridPosition + Vector3Int.up, out var topBlock))
            {
                stabilityUp = topBlock.GetStability();
            }

            if (_blockService.TryGetBlock(_gridPosition + Vector3Int.down, out var downBlock))
            {
                stabilityDown = downBlock.GetStability();
            }

            if (stabilityDown != 10)
            {
                stabilityDown--;
            }

            stabilityUp--;


            float supportVerticalStability = Math.Max(stabilityUp, stabilityDown);
            float stabilityNext = Math.Max(Math.Min(Math.Max(supportHorizontalStability, supportVerticalStability), 10),
                0);
            if (GetGridPosition().y == 0)
            {
                stabilityNext = 10;
            }

            return stabilityNext;
        }

        public Face GetFace(BlockFace face)
        {
            return face switch
            {
                BlockFace.Top => faces.top,
                BlockFace.Bottom => faces.bottom,
                BlockFace.Left => faces.left,
                BlockFace.Right => faces.right,
                BlockFace.Front => faces.center,
                BlockFace.Back => faces.back,
                _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
            };
        }

        public bool IsFalling()
        {
            return _isFalling;
        }

        public float GetStabilityGlue()
        {
            return _stabilityGlue;
        }

        public float GetStability()
        {
            return _stability;
        }

        [Button]
        private void GenerateBlock()
        {
            foreach (var child in transform.OfType<Transform>().ToArray())
            {
                DestroyImmediate(child.gameObject);
            }

            Face FaceGenerate(FaceScriptableObject face, Vector3 position, Quaternion quaternion)
            {
                var obj = PrefabUtility.InstantiatePrefab(face.GetFace(), this.transform) as GameObject;
                obj.transform.localPosition = position;
                obj.transform.localRotation = quaternion;
                return face.MakeFace();
            }

            _stabilityGlue = 9;
            _stability = 10;
            faces.top = FaceGenerate(blockScriptableObjects.top, Vector3.up / 2, Quaternion.identity);
            faces.bottom = FaceGenerate(blockScriptableObjects.bottom, Vector3.down / 2, Quaternion.Euler(180, 0, 0));

            faces.left = FaceGenerate(blockScriptableObjects.left, Vector3.left / 2, Quaternion.Euler(0, 0, 90));
            faces.right = FaceGenerate(blockScriptableObjects.right, Vector3.right / 2, Quaternion.Euler(0, 0, -90));

            faces.center = FaceGenerate(blockScriptableObjects.center, Vector3.forward / 2, Quaternion.Euler(90, 0, 0));
            faces.back = FaceGenerate(blockScriptableObjects.back, Vector3.back / 2, Quaternion.Euler(-90, 0, 0));
        }

        public float GetMass()
        {
            return _mass;
        }

        public float GetForceToOtherBlock(Block other)
        {
            return Math.Min(this.GetStabilityGlue(), other.GetStabilityGlue());
        }

        private BlockService _blockService;

        private bool _isFalling = false;
        private MinoKey _ownerMinoKey = MinoKey.NullMino;
    }
}