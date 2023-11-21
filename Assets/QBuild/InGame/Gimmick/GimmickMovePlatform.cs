using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Gimmick
{    
    [RequireComponent(typeof(Rigidbody))]
    public class GimmickMovePlatform : BaseGimmick
    {
        [SerializeField,Tooltip("��������")] private Vector3 _moveTransitionAxis = Vector3.zero;
        [SerializeField,Tooltip("��������")] private float _moveTransitionPeriod = 10.0f;
        [SerializeField,Tooltip("�������x")] private float _moveTransitionSpeed = 5.0f;
        [SerializeField] private bool _isMove = true;

        private Vector3 _initPosition;
        private Vector3 _targetPosition;
        private Vector3 _moveTargetPosition;
        private Rigidbody _myRB;
        private bool _reverse;

        private void Awake()
        {
            if(TryGetComponent<Rigidbody>(out _myRB)) Debug.LogError("Rigidbody���A�^�b�`����Ă��܂���B");
            _reverse = false;
            _myRB.useGravity = false;
            _initPosition = transform.position;
            _targetPosition = (_initPosition + (_moveTransitionAxis.normalized * _moveTransitionPeriod));
            _moveTargetPosition = _targetPosition;
        }
        
        
        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (!_isMove)
            {
                _myRB.velocity = Vector3.zero;
                return;
            }
            Vector3 moveVelo = _moveTransitionAxis.normalized * _moveTransitionSpeed;
            if (_reverse) moveVelo *= -1;
            _myRB.velocity = moveVelo;
            
            //�X�s�[�h�������ꍇ���]���Ȃ����
            if (Vector3.Distance(transform.position, _moveTargetPosition) <= 0.1f)
            {
                if (_reverse) _moveTargetPosition = _targetPosition;
                else _moveTargetPosition = _initPosition;
                _reverse = !_reverse;
            }
        }

        private void OnValidate()
        {
            _initPosition = transform.position;
            _targetPosition = _initPosition + (_moveTransitionAxis.normalized * _moveTransitionPeriod);
            Debug.Log("OnValidate");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 255, 0, 0.5f);
            Gizmos.DrawSphere(_initPosition, 0.2f);
            Gizmos.DrawSphere(_targetPosition, 0.2f);
            Gizmos.DrawLine(_initPosition, _targetPosition);
        }
        
        public override void Active()
        {
            //_isMove = true;
        }

        public override void Disable()
        {
            //_isMove = false;
        }
    }
}
