using System;
using QBuild.Behavior;
using QBuild.Utilities;
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
        public void SetPart(BlockPartScriptableObject part, Vector3 dir, Vector3 connectPosition,Matrix4x4 multiplePartAreaMatrix)
        {
            _part = part;
            var mesh = part.PartPrefab.GetComponent<MeshFilter>().sharedMesh;
            _meshFilter.sharedMesh = mesh;
            var tryPlaceInfo = new TryPlaceInfo(part, connectPosition, multiplePartAreaMatrix);

            if (PlacePartService.TryPlacePartPosition(tryPlaceInfo, out var outPartPosition))
            {
                transform.position = outPartPosition.GetPosition();
                Debug.Log($"Place {outPartPosition.GetPosition()}");
                Debug.Log($"Place transform {transform.position}");
                transform.rotation = outPartPosition.rotation;
                _keyIconSpriteRenderer.gameObject.SetActive(true);
                _keyIconSpriteRenderer.gameObject.transform.position = connectPosition + dir * 0.5f + Vector3.up * 0.25f;
            }
            else
            {
                _state = PartPlaceAreaState.NotEnoughSpace;
                _meshFilter.sharedMesh = null;
                _keyIconSpriteRenderer.gameObject.SetActive(false);
            }
        }

        public void Hide()
        {
            _meshFilter.sharedMesh = null;
            _keyIconSpriteRenderer.gameObject.SetActive(false);
        }

        public void SetKeyIcon(DirectionFRBL dir)
        {
            _keyIconSpriteRenderer.sprite = _icons.GetIcon(dir);
        }

        private void Start()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _keyIconSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        [SerializeField] private PartAreaIcons _icons;

        private MeshFilter _meshFilter;
        private BlockPartScriptableObject _part;
        private PartPlaceAreaState _state = PartPlaceAreaState.None;
        private SpriteRenderer _keyIconSpriteRenderer;
    }
}