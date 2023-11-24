using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.Core
{
    public class Movement : CoreComponent , IMover
    {
        public Vector3 currentVelocity { get; private set; }
        public bool canSetVelocity;

        private Vector3         _workspace;
        private Rigidbody       _myRB;

        public Vector3 CurrentMoverVelo { get; set; }
        public bool OnMover { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            _myRB = GetComponentInParent<Rigidbody>();
            if (_myRB == null) UnityEngine.Debug.LogError(transform.root.name + "‚ÉRigidBody‚ª‘¶Ý‚µ‚Ü‚¹‚ñB");
            _workspace = Vector3.zero;
            canSetVelocity = true;
        }

        public override void LogicUpdate()
        {
            currentVelocity = _myRB.velocity;
            
            if (OnMover)
            {
                _myRB.velocity += CurrentMoverVelo;
            }
        }

        #region SetFunction
        public void SetVelocityZero()
        {
            _workspace = Vector3.zero;
            SetFinalVelocity();
        }

        public void SetVelocity(Vector3 velocity, float speed)
        {
            _workspace = velocity.normalized * speed;
            //_workspace = new Vector3(velocity.x, 0, velocity.z).normalized * speed;
            SetFinalVelocity();
        }

        public void SetVelocity(Vector3 velocity)
        {
            _workspace = velocity;
            SetFinalVelocity();
        }

        public void SetVelocityXZ(Vector3 velocity, float speed)
        {
            _workspace = velocity.normalized * speed;
            _workspace.y = _myRB.velocity.y;
            SetFinalVelocity();
        }

        public void AddForce(Vector3 force, ForceMode mode)
        {
            _workspace = force;
            AddForceFinalVelocity(mode);
        }

        public Vector3 GetNowVelocity()
        {
            return _myRB.velocity;
        }

        private void SetFinalVelocity()
        {
            _myRB.velocity = _workspace;
            currentVelocity = _workspace;
            
            if (OnMover)
            {
                _myRB.velocity += CurrentMoverVelo;
            }
        }

        private void AddForceFinalVelocity(ForceMode mode)
        {
            if (!canSetVelocity) return;
            _myRB.AddForce(_workspace, mode);
            currentVelocity = _myRB.velocity;
        }

        public void SetCanVelocity(bool can) => canSetVelocity = can;

        public void SetLockVelocity(bool isLock)
        {
            if(isLock)
            {
                //_myRB.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                //_myRB.constraints = RigidbodyConstraints.FreezeRotation;
            }

        }
        #endregion

    }
}
