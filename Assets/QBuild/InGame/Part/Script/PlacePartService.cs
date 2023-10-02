using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QBuild.Part
{
    public static class PlacePartService
    {
        public static bool TryPlacePartPosition(BlockPartScriptableObject partScriptableObject, Vector3 connectPoint,
            out Vector3 outPartPosition)
        {
            var func = new Func<Vector3, bool>((pos) =>
            {
                var colliders = partScriptableObject.PartPrefab.GetComponentsInChildren<BoxCollider>();
                foreach (var boxCollider in colliders)
                {
                    if (!Physics.CheckBox(pos + boxCollider.center, (boxCollider.size / 2) * 0.9f,
                            boxCollider.transform.rotation, LayerMask.GetMask("Block"))) continue;
                    return false;
                }

                return true;
            });

            return TryPlacePartPosition(partScriptableObject, connectPoint, func, out outPartPosition);
        }

        public static bool TryPlacePartPosition(BlockPartScriptableObject partScriptableObject, Vector3 connectPoint,
            Func<Vector3, bool> checkCanPlaceFunc,
            out Vector3 outPartPosition)
        {
            outPartPosition = Vector3.zero;
            var newPartConnectPoint = partScriptableObject.PartPrefab.OnGetConnectPoints().ToArray();

            foreach (var point in newPartConnectPoint)
            {
                var newPartPosition = connectPoint - point;
                if (!checkCanPlaceFunc(newPartPosition)) continue;
                outPartPosition = newPartPosition;
                return true;
            }

            return false;
        }

        public static Vector3 FindClosestPointByAngleXZ(Vector3 from, Vector3 to, IEnumerable<Vector3> points)
        {
            // points の中心点を取得
            var center = points.Aggregate(Vector3.zero, (current, point) => current + point) / points.Count();
            from = center;
            return FindClosestPointByAngle(from, points, CalculateAngleXZ(from, from + to));
        }

        public static Vector3 FindClosestPointByAngle(Vector3 origin, IEnumerable<Vector3> points, float targetAngle)
        {
            var closestPoint = Vector3.zero;
            var smallestAngleDifference = float.MaxValue;

            foreach (var point in points)
            {
                var angle = CalculateAngleXZ(origin, point);

                var angleDifference = Mathf.Abs(Mathf.DeltaAngle(targetAngle, angle));

                // 最も小さい角度を更新しているか
                if (!(angleDifference < smallestAngleDifference)) continue;
                
                // 閾値以上の距離と離れているか (パーツを跨ぐ移動の際に接続点として選ばれないようにする)
                if(Vector3.Distance(origin, point) < 0.5f) continue;
                
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
        public static float CalculateAngleXZ(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2(to.z - from.z, to.x - from.x) * Mathf.Rad2Deg;
        }
    }
}