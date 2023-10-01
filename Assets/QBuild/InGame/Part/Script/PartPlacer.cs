using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        }


        private void ForwardPlacePart()
        {
            var partScriptableObject = _nextPartHolder.NextPart();
            PartView currentOnThePart = null;

            var hit = new RaycastHit[1];
            var size = Physics.RaycastNonAlloc(transform.position, Vector3.down, hit, 1f, LayerMask.GetMask("Block"));

            if (size <= 0) return;

            currentOnThePart = hit[0].collider.GetComponent<PartView>();
            var onThePartPosition = currentOnThePart.transform.position;
            var connectPoint = FindClosestPointByAngle(currentOnThePart.OnGetConnectPoints().Select(x => x + onThePartPosition),
                CalculateAngleXZ(transform.position, transform.position + transform.forward));

            var newPartConnectPoint = partScriptableObject.PartPrefab.OnGetConnectPoints().ToArray();
            
            Instantiate(partScriptableObject.PartPrefab, connectPoint - newPartConnectPoint[2], Quaternion.identity);
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
    }
}