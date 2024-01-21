using System;
using System.Collections;
using System.Collections.Generic;
using QBuild.Player;
using SherbetInspector.Core.Attributes;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickMovePlatform : BaseGimmick
    {
        [SerializeField, Tooltip("動く方向")] private Vector3 _moveTransitionAxis = Vector3.zero;
        [SerializeField, Tooltip("動く距離")] private float _moveTransitionPeriod = 10.0f;
        [SerializeField, Tooltip("動く速度")] private float _moveTransitionSpeed = 5.0f;
        [SerializeField] private bool _isMove = true;

        [SerializeField, Tooltip("補助線オブジェクト")] private GameObject _lineObject;
        
        private GimmickMovePlatformHelper _helper;
        
        private Vector3 _initPosition;
        private Vector3 _lastPosition;
        private float _time;

        private List<IMover> _movers = new List<IMover>();

        private void OnEnable()
        {
            if(_helper == null)return;
            _helper.AddMoverEvent += AddMover;
            _helper.RemoveMoverEvent += RemoveMover;
        }

        private void OnDisable()
        {
            if(_helper == null)return;
            _helper.AddMoverEvent -= AddMover;
            _helper.RemoveMoverEvent -= RemoveMover;
        }

        private void Awake()
        {
            _helper = GetComponentInChildren<GimmickMovePlatformHelper>();
            if (_helper == null) Debug.LogError($"{transform.name} : MovePlatformHelperが子オブジェクトに存在しません。");
            _initPosition = transform.position;
            _lastPosition = _initPosition;
            _time = 0.0f;
        }


        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            if (!_isMove)
            {
                foreach (IMover mover in _movers)
                {
                    mover.SetMoverVelocity(Vector3.zero);
                }

                return;
            }
            Vector3 moveAxis = _moveTransitionAxis.normalized * _moveTransitionPeriod;
            Vector3 goalPosition = (_initPosition + moveAxis +
                                   (moveAxis * Mathf.Sin(_time * _moveTransitionSpeed)));
            transform.position = goalPosition;
            
            Vector3 moveAmount = goalPosition - _lastPosition;
            _lastPosition = goalPosition;

            foreach (IMover mover in _movers)
            {
                mover.AddMoverPosition(moveAmount);
            }
            
            _time += Time.deltaTime;
        }

        private void OnValidate()
        {
            //_initPosition = transform.position;
            //_targetPosition = _initPosition + (_moveTransitionAxis.normalized * _moveTransitionPeriod);
        }

        private void OnDrawGizmosSelected()
        {
            var reset = false;
            Vector3 _targetPosition = transform.TransformPoint(_moveTransitionAxis.normalized * _moveTransitionPeriod * 2.0f);
            if (_initPosition == Vector3.zero)
            {
                _initPosition = transform.position;
                reset = true;
            }

            Gizmos.color = new Color(0, 255, 0, 0.5f);
            Gizmos.DrawSphere(_initPosition, 0.2f);
            Gizmos.DrawSphere(_targetPosition, 0.2f);
            Gizmos.DrawLine(_initPosition, _targetPosition);

            if (reset)
            {
                _initPosition = Vector3.zero;
            }
        }

        public void AddMover(IMover mover)
        {
            Debug.Log("AddMover");
            _movers.Add(mover);
        }
        public void RemoveMover(IMover mover){_movers.Remove(mover);}
        
        public override void Active()
        {
            _isMove = true;
        }

        public override void Disable()
        {
            _isMove = false;
        }

        [Button]
        private void GenerateLine()
        {
            if (_lineObject.TryGetComponent(out MoveLine moveLine))
            {
                moveLine.GenerateLine(transform.position, transform.TransformPoint(_moveTransitionAxis.normalized * _moveTransitionPeriod * 2.0f));
            }
        }
    }
}