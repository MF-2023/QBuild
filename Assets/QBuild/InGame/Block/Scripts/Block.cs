using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Mino;
using SherbetInspector.Core.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
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


    public class Block : MonoBehaviour
    {
        [SerializeField] private BlockType blockScriptableObjects;

        [SerializeField] private Faces faces;


        [SerializeField] private Vector3Int _gridPosition;

        [SerializeField] private bool isFalling = true;

        [SerializeField] private float stabilityGlue = 0;
        [SerializeField] private float stability = 0;
        [SerializeField] private float mass = 1;

        [Inject]
        private void Inject(BlockUseCase blockUseCase)
        {
            Debug.Log("Inject Block");
            _blockUseCase = blockUseCase;
        }

        public void GenerateBlock(BlockType type, Vector3Int pos)
        {
            _ownerMinoKey = MinoKey.NullMino; 
            
            blockScriptableObjects = type;
            _gridPosition = pos;
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
            return _blockUseCase.CanPlace(_gridPosition + move);
        }

        public void MoveNext(Vector3Int move)
        {
            var before = _gridPosition;
            _gridPosition += move;
            transform.localPosition = _gridPosition;
            _blockUseCase.UpdateBlock(this, before);

            name = $"Block_{_gridPosition}";
        }

        public Vector3Int GetGridPosition()
        {
            return _gridPosition;
        }


        public void OnBlockPlaced(float stabilityNext = -1)
        {
            isFalling = false;

            if (stabilityNext < 0) stabilityNext = CalcStability();

            stability = stabilityNext;
            Debug.Log("Place");
        }

        private float CalcStability()
        {
            float supportHorizontalStability = 0;

            foreach (var t in Vector3IntDirs.HorizontalDirections)
            {
                if (_blockUseCase.TryGetBlock(_gridPosition + t, out var block))
                {
                    supportHorizontalStability = Math.Max(block.GetStability(), supportHorizontalStability);
                }
            }

            supportHorizontalStability--;

            float stabilityUp = 0;
            float stabilityDown = 0;

            if (_blockUseCase.TryGetBlock(_gridPosition + Vector3Int.up, out var topBlock))
            {
                stabilityUp = topBlock.GetStability();
            }

            if (_blockUseCase.TryGetBlock(_gridPosition + Vector3Int.down, out var downBlock))
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

        public void GlueBlock(BlockFace blockFace, Block block)
        {
            switch (blockFace)
            {
                case BlockFace.Top:
                    faces.top.SetGlueBlock(block);
                    break;
                case BlockFace.Bottom:
                    faces.bottom.SetGlueBlock(block);
                    break;
                case BlockFace.Left:
                    faces.left.SetGlueBlock(block);
                    break;
                case BlockFace.Right:
                    faces.right.SetGlueBlock(block);
                    break;
                case BlockFace.Front:
                    faces.center.SetGlueBlock(block);
                    break;
                case BlockFace.Back:
                    faces.back.SetGlueBlock(block);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blockFace), blockFace, null);
            }
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
            return isFalling;
        }

        public float GetStabilityGlue()
        {
            return stabilityGlue;
        }

        public float GetStability()
        {
            return stability;
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

            stabilityGlue = 9;
            stability = 10;
            faces.top = FaceGenerate(blockScriptableObjects.top, Vector3.up / 2, Quaternion.identity);
            faces.bottom = FaceGenerate(blockScriptableObjects.bottom, Vector3.down / 2, Quaternion.Euler(180, 0, 0));

            faces.left = FaceGenerate(blockScriptableObjects.left, Vector3.left / 2, Quaternion.Euler(0, 0, 90));
            faces.right = FaceGenerate(blockScriptableObjects.right, Vector3.right / 2, Quaternion.Euler(0, 0, -90));

            faces.center = FaceGenerate(blockScriptableObjects.center, Vector3.forward / 2, Quaternion.Euler(90, 0, 0));
            faces.back = FaceGenerate(blockScriptableObjects.back, Vector3.back / 2, Quaternion.Euler(-90, 0, 0));
        }

        public float GetMass()
        {
            return mass;
        }

        public float GetForceToOtherBlock(Block _other)
        {
            return Math.Min(this.GetStabilityGlue(), _other.GetStabilityGlue());
        }

        private BlockUseCase _blockUseCase;

        private MinoKey _ownerMinoKey = MinoKey.NullMino;
    }
}