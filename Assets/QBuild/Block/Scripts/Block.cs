using System;
using System.Linq;
using SherbetInspector.Core.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private BlockGenerator blockScriptableObjects;

        [SerializeField] private Faces faces;

        [SerializeField] private Vector3Int _gridPosition;

        [SerializeField] private bool isFalling = true;
        
        [SerializeField] private float stability = 0;
        [SerializeField] private float mass = 1;

        private static BlockManager _blockManager;

        public static void Init(BlockManager manager)
        {
            _blockManager = manager;
        }

        public void GenerateBlock(BlockGenerator generator, Vector3Int pos)
        {
            blockScriptableObjects = generator;
            _gridPosition = pos;
            GenerateBlock();
            _blockManager.UpdateBlock(this);
        }

        public bool CanMove(Vector3Int move)
        {
            return _blockManager.CanPlace(_gridPosition + move);
        }
        
        public void MoveNext(Vector3Int move)
        {
            var before = _gridPosition;
            _gridPosition += move;
            transform.localPosition = _gridPosition;
            _blockManager.UpdateBlock(this, before);
        }

        public Vector3Int GetGridPosition()
        {
            return _gridPosition;
        }


        public void OnBlockPlaced()
        {
            isFalling = false;
            Debug.Log("Place");
        }

        public void GlueBlock(BlockFace blockFace,Block block)
        {
            switch (blockFace)
            {
                case BlockFace.Top:
                    faces.top.SetGlueBlock(block);
                    break;
                case BlockFace.Bottom:
                    faces.bottom.SetGlueBlock(block);
                    break;
                case BlockFace.West:
                    faces.left.SetGlueBlock(block);
                    break;
                case BlockFace.East:
                    faces.right.SetGlueBlock(block);
                    break;
                case BlockFace.South:
                    faces.center.SetGlueBlock(block);
                    break;
                case BlockFace.North:
                    faces.back.SetGlueBlock(block);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(blockFace), blockFace, null);
            }
        }
        
        public bool IsFalling()
        {
            return isFalling;
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

            stability = 40;
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
    }
}