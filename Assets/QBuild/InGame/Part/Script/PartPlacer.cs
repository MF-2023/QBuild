using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace QBuild.Part
{
    public class PartPlacer : MonoBehaviour
    {
        [Inject]
        void Inject(@InputSystem inputSystem,NextPartHolder nextPartHolder)
        {
            inputSystem.InGame.BlockPlaceF.performed += _ => ForwardPlacePart();
            inputSystem.InGame.BlockPlaceR.performed += _ => RightPlacePart();
            inputSystem.InGame.BlockPlaceB.performed += _ => BackPlacePart();
            inputSystem.InGame.BlockPlaceL.performed += _ => LeftPlacePart();
            _nextPartHolder = nextPartHolder;
        }

        private void Start()
        {
            _nextPartScriptableObject = _nextPartHolder.NextPart();
        }

        private void Update()
        {
            OnThePartUpdate();
        }

        private void ForwardPlacePart()
        {
            DirPlacePart(Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized);
        }

        private void RightPlacePart()
        {
            DirPlacePart(UnityEngine.Camera.main.transform.right);
        }

        private void BackPlacePart()
        {
            DirPlacePart(UnityEngine.Camera.main.transform.forward * -1);
        }

        private void LeftPlacePart()
        {
            DirPlacePart(UnityEngine.Camera.main.transform.right * -1);
        }

        private void DirPlacePart(Vector3 dir)
        {
            var onThePartPosition = CurrentOnThePart.transform.position;
            var connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, dir,
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            Place(connectPoint);
        }

        private void Place(Vector3 connectPoint)
        {
            // 次の設置するパーツを取得
            var partScriptableObject = _nextPartScriptableObject;

            if (PlacePartService.TryPlacePartPosition(partScriptableObject, connectPoint, out var outPartPosition))
            {
                _nextPartScriptableObject = _nextPartHolder.NextPart();
                Instantiate(partScriptableObject.PartPrefab, outPartPosition, Quaternion.identity);
                OnThePartChanged();
            }
        }

        private void OnThePartUpdate()
        {
            var hit = new RaycastHit[1];
            var size = Physics.RaycastNonAlloc(transform.position, Vector3.down, hit, 1f, LayerMask.GetMask("Block"));
            if (size > 0)
            {
                CurrentOnThePart = hit[0].collider.GetComponentInParent<PartView>();
            }
            else
            {
                CurrentOnThePart = null;
            }
        }

        private void OnThePartChanged()
        {
            _multiplePartArea.UpdatePart(transform.position, _currentOnThePart, _nextPartScriptableObject);
        }

        [SerializeField] private PartListScriptableObject _partListScriptableObject;
        [SerializeField] private NextPartHolder _nextPartHolder;
        [SerializeField] private BlockPartScriptableObject _nextPartScriptableObject;

        [SerializeField] private PartView _currentOnThePart;
        [SerializeField] private MultiplePartArea _multiplePartArea;
        private PartView CurrentOnThePart
        {
            get => _currentOnThePart;
            set
            {
                if (_currentOnThePart == value) return;
                _currentOnThePart = value;
                OnThePartChanged();
            }
        }
    }
}