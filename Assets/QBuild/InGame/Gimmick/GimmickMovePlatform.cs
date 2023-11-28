using System;
using System.Collections;
using System.Collections.Generic;
using QBuild.Player;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickMovePlatform : BaseGimmick
    {
        [SerializeField, Tooltip("“®‚­•ûŒü")] private Vector3 _moveTransitionAxis = Vector3.zero;
        [SerializeField, Tooltip("“®‚­‹——£")] private float _moveTransitionPeriod = 10.0f;
        [SerializeField, Tooltip("“®‚­‘¬“x")] private float _moveTransitionSpeed = 5.0f;
        [SerializeField] private bool _isMove = true;

        private Vector3 _initPosition;
        private Vector3 _lastPosition;
        private bool _reverse;

        private List<IMover> _movers = new List<IMover>();

        private void Awake()
        {
            _reverse = false;
            _initPosition = transform.position;
            _lastPosition = _initPosition;
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
                                   (moveAxis * Mathf.Sin(Time.time * _moveTransitionSpeed)));
            transform.position = goalPosition;
            
            Vector3 moveAmount = goalPosition - _lastPosition;
            _lastPosition = goalPosition;

            foreach (IMover mover in _movers)
            {
                mover.AddMoverPosition(moveAmount);
            }
        }

        private void OnValidate()
        {
            //_initPosition = transform.position;
            //_targetPosition = _initPosition + (_moveTransitionAxis.normalized * _moveTransitionPeriod);
        }

        private void OnDrawGizmosSelected()
        {
            /*
            Gizmos.color = new Color(0, 255, 0, 0.5f);
            Gizmos.DrawSphere(_initPosition, 0.2f);
            Gizmos.DrawSphere(_targetPosition, 0.2f);
            Gizmos.DrawLine(_initPosition, _targetPosition);
            */
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent<PlayerAdapter>(out PlayerAdapter adapter))
            {
                adapter.OnMoverEnter();
                _movers.Add(adapter);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.transform.TryGetComponent<PlayerAdapter>(out PlayerAdapter adapter))
            {
                adapter.OnMoverExit();
                _movers.Remove(adapter);
            }
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