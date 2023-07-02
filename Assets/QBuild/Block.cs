using System;
using System.Linq;
using QBuild.Face;
using SherbetInspector.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace QBuild
{

    
    [Serializable]
    public struct Faces
    {
        public FaceScriptableObject top;
        public FaceScriptableObject center;
        public FaceScriptableObject right;
        public FaceScriptableObject back;
        public FaceScriptableObject left;
        public FaceScriptableObject bottom;
    }
    
    public class Block : MonoBehaviour
    {
        [SerializeField]
        private Faces faces;
        
        [Button]
        private void GenerateBlock()
        {
            foreach(var child in transform.OfType<Transform>().ToArray())
            {
                DestroyImmediate(child.gameObject);
            }
            void FaceGenerate(FaceScriptableObject face, Vector3 position,Quaternion quaternion)
            {
                var obj = PrefabUtility.InstantiatePrefab(face.GetFace(), this.transform) as GameObject;
                obj.transform.localPosition = position;
                obj.transform.localRotation = quaternion;
            }

            FaceGenerate(faces.top, Vector3.up / 2,Quaternion.identity);
            FaceGenerate(faces.bottom, Vector3.down / 2,Quaternion.Euler(180,0,0));
            
            FaceGenerate(faces.left, Vector3.left / 2,Quaternion.Euler(0,0,90));
            FaceGenerate(faces.right, Vector3.right / 2,Quaternion.Euler(0,0,-90));
            
            FaceGenerate(faces.center, Vector3.forward / 2,Quaternion.Euler(90,0,0));
            FaceGenerate(faces.back, Vector3.back / 2,Quaternion.Euler(-90,0,0));
        }
    }
}
