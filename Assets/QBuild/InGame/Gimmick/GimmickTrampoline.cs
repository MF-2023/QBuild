using UnityEngine;
using DG.Tweening;

namespace QBuild.Gimmick
{
    public class GimmickTrampoline : BaseGimmick
    {
        [SerializeField,Tooltip("目標ポイント")]                                  private Transform _targetPoint;
        [SerializeField,Tooltip("中央ポイント（nullの場合自動で計算される）")]    private Transform _centerPoint;
        [SerializeField,Tooltip("中央ポイントが設定しない場合の中央の高さ")]      private float     _jumpHeight = 0;
        [SerializeField,Tooltip("目標ポイントに到着するまでの時間")]              private float     _time;

        public override void Active()
        {
            //プレイヤーを何かしらの方法で取得する
            Transform HogePlayer = null;

            //TODO::西田::プレイヤーのSimulationを切る
            Vector3 startPosition = HogePlayer.position;
            Vector3 endPosition = _targetPoint.position;
            Vector3 centerPosition;
            if (_centerPoint != null) centerPosition = _centerPoint.position;
            else centerPosition = GetCenterPoint(startPosition, endPosition, _jumpHeight);

            HogePlayer.DOLocalPath(
                new[]
                {
                    startPosition,
                    centerPosition,
                    endPosition,
                },
                _time, PathType.CubicBezier, gizmoColor: Color.red)
                .SetOptions(false);
        }

        public override void Disable()
        {
        }

        private Vector3 GetCenterPoint(Vector3 startPoint,Vector3 endPoint,float height)
        {
            Vector3 ret = Vector3.zero;
            ret = (endPoint + startPoint) * 0.5f;
            ret.y += height;
            return ret;
        }
    }
}
