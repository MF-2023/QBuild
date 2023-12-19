using System;
using DG.Tweening;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickDoor : BaseGimmick
    {
        [SerializeField] private Transform _moveTransform;

        [SerializeField] private Vector3 _openPosition;
        [SerializeField] private Vector3 _closePosition;

        [SerializeField] private bool _isOpened = false;

        private Tweener _doorAnimation;

        public override void Active()
        {
            Open();
        }

        public override void Disable()
        {
        }

        public void Open()
        {
            if (_isOpened) return;
            _doorAnimation = _moveTransform.DOLocalMoveY(_openPosition.y, 1);
            _isOpened = true;
        }

        public void Close()
        {
            if (!_isOpened) return;
            _doorAnimation = _moveTransform.DOLocalMoveY(_closePosition.y, 1);
            _isOpened = false;
        }

        private void OnValidate()
        {
            var pos = _moveTransform.localPosition;
            var targetY = _isOpened ? _openPosition.y : _closePosition.y;
            _moveTransform.localPosition = new Vector3(pos.x, targetY, pos.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(_openPosition), 0.05f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.TransformPoint(_closePosition), 0.05f);
        }
    }
}