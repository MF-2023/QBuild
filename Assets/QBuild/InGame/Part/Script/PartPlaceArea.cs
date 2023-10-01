using System;
using UnityEngine;

namespace QBuild.Part
{
    public class PartPlaceArea : MonoBehaviour
    {
        public void SetPart(BlockPartScriptableObject part, Vector3 connectPosition)
        {
            _part = part;
            var mesh = part.PartPrefab.GetComponent<MeshFilter>().sharedMesh;
            _meshFilter.sharedMesh = mesh;
            if (PlacePartService.TryPlacePartPosition(part, connectPosition, out var outPartPosition))
            {
                transform.position = outPartPosition;
            }
        }

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        private MeshFilter _meshFilter;
        private BlockPartScriptableObject _part;
    }
}