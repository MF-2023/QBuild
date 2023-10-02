using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace QBuild.Part
{
    public class PartPlacer : MonoBehaviour
    {
        [Inject]
        void Inject(@InputSystem inputSystem)
        {
            inputSystem.InGame.BlockPlaceF.performed += _ => ForwardPlacePart();
            inputSystem.InGame.BlockPlaceR.performed += _ => RightPlacePart();
            inputSystem.InGame.BlockPlaceB.performed += _ => BackPlacePart();
            inputSystem.InGame.BlockPlaceL.performed += _ => LeftPlacePart();
        }

        private void Start()
        {
            _nextPartHolder = new NextPartHolder(_partListScriptableObject);
            _nextPartScriptableObject = _nextPartHolder.NextPart();

            _partPlaceAreaN = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaN.name += ":N";

            _partPlaceAreaE = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaE.name += ":E";

            _partPlaceAreaW = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaW.name += ":W";

            _partPlaceAreaS = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaS.name += ":S";
        }

        private void Update()
        {
            OnThePartUpdate();
        }

        private void ForwardPlacePart()
        {
            Debug.Log(Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)));
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
            _nextPartScriptableObject = _nextPartHolder.NextPart();

            if (PlacePartService.TryPlacePartPosition(partScriptableObject, connectPoint, out var outPartPosition))
            {
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
            var onThePartPosition = _currentOnThePart.transform.position;
            var connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(0, 0, 1),
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            _partPlaceAreaN.SetPart(_nextPartScriptableObject, connectPoint);

            connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(1, 0, 0),
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            _partPlaceAreaE.SetPart(_nextPartScriptableObject, connectPoint);

            connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(0, 0, -1),
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            _partPlaceAreaW.SetPart(_nextPartScriptableObject, connectPoint);

            connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(-1, 0, 0),
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            _partPlaceAreaS.SetPart(_nextPartScriptableObject, connectPoint);
        }

        [SerializeField] private PartListScriptableObject _partListScriptableObject;
        [SerializeField] private NextPartHolder _nextPartHolder;
        [SerializeField] private BlockPartScriptableObject _nextPartScriptableObject;
        [SerializeField] private PartPlaceArea _partPlaceAreaPrefab;

        private PartPlaceArea _partPlaceAreaN;
        private PartPlaceArea _partPlaceAreaE;
        private PartPlaceArea _partPlaceAreaW;
        private PartPlaceArea _partPlaceAreaS;

        [SerializeField] private PartView _currentOnThePart;

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