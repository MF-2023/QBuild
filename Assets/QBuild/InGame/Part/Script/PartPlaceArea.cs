using System;
using UnityEngine;

namespace QBuild.Part
{
    public enum PartPlaceAreaState
    {
        None,
        CanPlace,
        
        NotEnoughSpace,
        CannotConnect,
    }
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
            else
            {
                _state = PartPlaceAreaState.NotEnoughSpace;
                _meshFilter.sharedMesh = null;
            }
        }

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
        }

        private MeshFilter _meshFilter;
        private BlockPartScriptableObject _part;
        private PartPlaceAreaState _state = PartPlaceAreaState.None;
    }
}