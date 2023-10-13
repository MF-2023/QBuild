using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QBuild.Part
{
    public struct TryPlaceInfo
    {
        public BlockPartScriptableObject partScriptableObject;
        public Vector3 connectPoint;
        public Matrix4x4 multipleMatrix;

        public TryPlaceInfo(BlockPartScriptableObject partScriptableObject, Vector3 connectPoint,
            Matrix4x4 multipleMatrix)
        {
            this.partScriptableObject = partScriptableObject;
            this.connectPoint = connectPoint;
            this.multipleMatrix = multipleMatrix;
        }
    }


    public static class PlacePartService
    {
        public static bool TryPlacePartPosition(TryPlaceInfo info,
            out Matrix4x4 outPartMatrix)
        {
            var func = new Func<Matrix4x4, bool>((matrix) =>
            {
                var colliders = info.partScriptableObject.PartPrefab.GetComponentsInChildren<BoxCollider>();
                foreach (var boxCollider in colliders)
                {
                    var pos = matrix.MultiplyPoint(boxCollider.center);
                    var rot = matrix.rotation * boxCollider.transform.rotation;
                    if (!Physics.CheckBox(pos, (boxCollider.size / 2) * 0.9f,
                            rot, LayerMask.GetMask("Block"))) continue;
                    return false;
                }

                return true;
            });

            return TryPlacePartPosition(info, func, out outPartMatrix);
        }

        public static bool TryPlacePartPosition(TryPlaceInfo info,
            Func<Matrix4x4, bool> checkCanPlaceFunc,
            out Matrix4x4 outPartMatrix)
        {
            outPartMatrix = Matrix4x4.zero;
            var newPartConnectPoint = info.partScriptableObject.PartPrefab.OnGetConnectPoints().ToArray();

            foreach (var point in newPartConnectPoint)
            {
                // 現在の接続点から目的の接続点へのオフセットを計算します。
                var offset = info.connectPoint - point;

                // 最初の変換を作成してオフセットを適用します。
                var matrix = Matrix4x4.TRS(offset, Quaternion.identity, Vector3.one);

                // 既存のパーツの回転を適用します。
                matrix *= Matrix4x4.Rotate(info.multipleMatrix.rotation);

                // この変換で新しいパーツの接続点がどこに移動するかを計算します。
                var transformedPoint = matrix.MultiplyPoint(point);

                // 目的の接続点から変換後の接続点までのオフセットを計算します。
                var rotateOffset = -info.connectPoint + transformedPoint;

                Debug.Log($"rotateOffset {rotateOffset}");
                Debug.Log($"matPoint {matrix.GetPosition()}");
                // このオフセットを適用して正確な位置に調整します。
                matrix = Matrix4x4.TRS(matrix.GetPosition() - rotateOffset, matrix.rotation, Vector3.one);
                Debug.Log($"matPoint {matrix.GetPosition()}");

                // 新しい変換で新しいパーツが配置できるかどうかをチェックします。
                if (checkCanPlaceFunc(matrix))
                {
                    Debug.Log("Place");
                    outPartMatrix = matrix;
                    return true;
                }
            }

            return false;

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
                if (Vector3.Distance(origin, point) < 0.5f) continue;

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