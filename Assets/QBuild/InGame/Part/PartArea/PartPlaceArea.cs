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
        public void SetPart(BlockPartScriptableObject part, Vector3 dir, Vector3 connectPosition,
            Matrix4x4 multiplePartAreaMatrix)
        {
            _part = part;
            var mesh = part.PartPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            _meshFilter.sharedMesh = mesh;
            var tryPlaceInfo = new TryPlaceInfo(part, dir, connectPosition, multiplePartAreaMatrix);

            if (PlacePartService.TryPlacePartPosition(tryPlaceInfo, out var outPartPosition))
            {
                var contact = false;
                foreach (var magnet in part.PartPrefab.OnGetMagnets())
                {
                    var position = outPartPosition.MultiplyPoint(magnet.Position);
                    if (connectPosition != position)
                    {
                        var dirRay = magnet.Direction.ToVector3();
                        dirRay = outPartPosition.rotation * dirRay;
                        var ray = new Ray(position - dirRay * 0.1f, dirRay.normalized * 0.2f);
                        if (!Physics.Raycast(ray, out var hit, 1f, LayerMask.GetMask("Block"))) continue;
                        var hitPart = hit.collider.GetComponentInParent<PartView>();
                        if (hitPart == null) continue;
                        contact = true;
                        break;
                    }
                }

                GetComponentInChildren<Renderer>().material.SetColor("_WireframeColor", contact ? Color.green : Color.blue);
            }
            else
            {
                _state = PartPlaceAreaState.NotEnoughSpace;
                GetComponentInChildren<Renderer>().material.SetColor("_WireframeColor", Color.red);
            }

            transform.position = outPartPosition.GetPosition();
            transform.rotation = outPartPosition.rotation;
            _keyIconSpriteRenderer.gameObject.SetActive(true);
            _keyIconSpriteRenderer.gameObject.transform.position =
                connectPosition + dir * 0.5f + Vector3.up * 0.25f;
        }

        public void Hide()
        {
            _meshFilter.sharedMesh = null;
            _keyIconSpriteRenderer.gameObject.SetActive(false);
        }

        public void SetKeyIcon(DirectionFRBL dir)
        {
            if (_keyIconSpriteRenderer == null) return;
            Debug.DebugBreak();
            _keyIconSpriteRenderer.sprite = _icons.GetIcon(dir);
        }

        private void Start()
        {
            _meshFilter = GetComponentInChildren<MeshFilter>();
            _keyIconSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        [SerializeField] private PartAreaIcons _icons;

        private MeshFilter _meshFilter;
        private BlockPartScriptableObject _part;
        private PartPlaceAreaState _state = PartPlaceAreaState.None;
        private SpriteRenderer _keyIconSpriteRenderer;
    }
}