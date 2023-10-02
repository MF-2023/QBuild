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
            inputSystem.InGame.BlockDone.performed += _ => ForwardPlacePart();
        }

        private void Start()
        {
            _nextPartHolder = new NextPartHolder(_partListScriptableObject);
            _nextPartScriptableObject = _nextPartHolder.NextPart();

            _partPlaceAreaN = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaE = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaW = Instantiate(_partPlaceAreaPrefab);
            _partPlaceAreaS = Instantiate(_partPlaceAreaPrefab);
        }

        private void Update()
        {
            OnThePartUpdate();
        }

        private void ForwardPlacePart()
        {
            // 次の設置するパーツを取得
            var partScriptableObject = _nextPartScriptableObject;
            _nextPartScriptableObject = _nextPartHolder.NextPart();


            var onThePartPosition = CurrentOnThePart.transform.position;
            var connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, transform.forward,
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));

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
            var connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(0,0,1),
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            _partPlaceAreaN.SetPart(_nextPartScriptableObject, connectPoint);
            
            connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(1,0,0),
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            _partPlaceAreaE.SetPart(_nextPartScriptableObject, connectPoint);

            connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(0,0,-1),
                CurrentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition));
            _partPlaceAreaW.SetPart(_nextPartScriptableObject, connectPoint);

            connectPoint = PlacePartService.FindClosestPointByAngleXZ(transform.position, new Vector3(-1,0,0),
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