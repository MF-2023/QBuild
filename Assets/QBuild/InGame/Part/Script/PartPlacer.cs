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
            _partPlaceArea = Instantiate(_partPlaceAreaPrefab);
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

            // 乗ってるブロックを取得
            PartView currentOnThePart = null;
            var hit = new RaycastHit[1];
            var size = Physics.RaycastNonAlloc(transform.position, Vector3.down, hit, 1f, LayerMask.GetMask("Block"));
            if (size <= 0) return;
            currentOnThePart = hit[0].collider.GetComponentInParent<PartView>();
            
            var onThePartPosition = currentOnThePart.transform.position;
            var connectPoint = FindClosestPointByAngle(
                currentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition),
                CalculateAngleXZ(transform.position, transform.position + transform.forward));

            if (PlacePartService.TryPlacePartPosition(partScriptableObject, connectPoint, out var outPartPosition))
            {
                Instantiate(partScriptableObject.PartPrefab, outPartPosition, Quaternion.identity);
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
            var connectPoint = FindClosestPointByAngle(
                _currentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition),
                CalculateAngleXZ(transform.position, transform.position + transform.forward));

            _partPlaceArea.SetPart(_nextPartScriptableObject, connectPoint);
        }

        private bool CanPlacePart()
        {
            return true;
        }

        private Vector3 FindClosestPointByAngle(IEnumerable<Vector3> points, float targetAngle)
        {
            var closestPoint = Vector3.zero;
            var smallestAngleDifference = float.MaxValue;

            var playerPos = transform.position;

            foreach (var point in points)
            {
                var angle = CalculateAngleXZ(playerPos, point);

                var angleDifference = Mathf.Abs(Mathf.DeltaAngle(targetAngle, angle));

                if (!(angleDifference < smallestAngleDifference)) continue;
                smallestAngleDifference = angleDifference;
                closestPoint = point;
            }

            return closestPoint;
        }

        /// <summary>
        /// 2つの点の角度をXZ面上で計算する
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private float CalculateAngleXZ(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2(to.z - from.z, to.x - from.x) * Mathf.Rad2Deg;
        }

        [SerializeField] private PartListScriptableObject _partListScriptableObject;
        [SerializeField] private NextPartHolder _nextPartHolder;
        [SerializeField] private BlockPartScriptableObject _nextPartScriptableObject;
        [SerializeField] private PartPlaceArea _partPlaceAreaPrefab;
        private PartPlaceArea _partPlaceArea;
        [SerializeField] private PartView _currentOnThePart;

        private PartView CurrentOnThePart
        {
            get => _currentOnThePart;
            set
            {
                if(_currentOnThePart == value) return;
                _currentOnThePart = value;
                OnThePartChanged();
            }
        }
    }
}