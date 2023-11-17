using UnityEngine;
using DG.Tweening;
using System;
using QBuild.Gimmick.Effector;
using QBuild.Gimmick.TriggerPattern;
using QBuild.Player.Controller;
using UnityEngine.Serialization;

namespace QBuild.Gimmick
{
    public class GimmickTrampoline : BaseGimmick
    {
        [SerializeField, Tooltip("目標ポイント")] private Transform _targetPoint;
        [SerializeField, Tooltip("目標ポイントに到着するまでの時間")] private float _time;

        [SerializeField, Tooltip("曲線の角度")] private float _angle;
        [SerializeField, Tooltip("曲線の大きさ")] private float _distance;

        private Transform _contactTran;
        private Vector3 _controlPoint1 = Vector3.zero;
        private Vector3 _controlPoint2 = Vector3.zero;

        public override void Active()
        {
            this.OnContacted(x =>
            {
                Debug.Log(("OnContacted"));
                _contactTran = x.Target.transform;
            });
            
            CalControlPoint(_contactTran.position);
            Vector3[] wayPoints = new[]
            {
                _targetPoint.position,
                _controlPoint1,
                _controlPoint2
            };

            _contactTran.DOPath(wayPoints, _time, PathType.CubicBezier, PathMode.Full3D, 10, Color.red)
                .SetEase(Ease.OutExpo);
        }

        public override void Disable()
        {
        }

        public void CalControlPoint(Vector3 startPoint)
        {
            Vector3 endPoint = _targetPoint.position;

            //2点の角度を求める
            float rad = Mathf.Atan2(endPoint.z - startPoint.z, endPoint.x - startPoint.x);

            Vector3 vec1ang = new Vector3(Mathf.Cos(_angle) * Mathf.Cos(rad), Mathf.Sin(_angle),
                Mathf.Cos(_angle) * Mathf.Sin(rad));
            Vector3 vec2ang = new Vector3(Mathf.Cos(_angle) * -Mathf.Cos(rad), Mathf.Sin(_angle),
                Mathf.Cos(_angle) * -Mathf.Sin(rad));

            _controlPoint1 = startPoint + vec1ang * _distance;
            _controlPoint2 = endPoint + vec2ang * _distance;
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;
            CalControlPoint(position);
            DrawBezierCurve(position, _targetPoint.position, _controlPoint1, _controlPoint2);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawSphere(_controlPoint1, 0.5f);
            Gizmos.DrawSphere(_controlPoint2, 0.5f);
        }

        private void DrawBezierCurve(Vector3 startPoint, Vector3 endPoint, Vector3 controlPoint1, Vector3 controlPoint2)
        {
            Gizmos.color = Color.white;

            Vector3 lastPoint = startPoint;

            //曲線の解像度
            int resolution = 10;
            for (int i = 1; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                Vector3 currentPoint = CalculateCubicBezierPoint(t, startPoint, controlPoint1, controlPoint2, endPoint);
                Gizmos.DrawLine(lastPoint, currentPoint);
                lastPoint = currentPoint;
            }
        }

        private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }
    }
}