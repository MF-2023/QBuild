using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

namespace QBuild.Player.Core
{
    public class Movement : CoreComponent
    {
        public Vector3 currentVelocity { get; private set; }
        public Vector3 CurrentMoverVelocity { get; set; }
        public bool OnMover;
        public bool canSetVelocity;

        private Vector3         _workspace;
        private Rigidbody       _myRB;
        private PhysicMaterial _myPM;
        
        protected override void Awake()
        {
            base.Awake();
            _myRB = GetComponentInParent<Rigidbody>();
            if (_myRB == null) UnityEngine.Debug.LogError(transform.root.name + "Ç…RigidBodyÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅB");
            _workspace = Vector3.zero;
            canSetVelocity = true;
            OnMover = false;
        }

        public override void CompLogicUpdate()
        {
            currentVelocity = _myRB.velocity;
        }

        public override void CompFixedUpdate()
        {
            //currentVelocity = _myRB.velocity;
            if (OnMover)
            {
                _myRB.velocity = CurrentMoverVelocity;
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
                _myRB.velocity += CurrentMoverVelocity;
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

            SetPhysicMaterial(isLock);
        }

        public void SetPhysicMaterial(PhysicMaterial pm) { _myPM = pm;}

        /// <summary>
        /// ääÇËÇ‚Ç∑Ç≥ÇïœçXÇ∑ÇÈ
        /// </summary>
        /// <param name="flg">TRUE : ääÇÁÇ»Ç¢ FALSE : ääÇÈ</param>
        private void SetPhysicMaterial(bool flg)
        {
            if (_myPM == null) return;
            if (flg)
            {
                _myPM.dynamicFriction = 1.0f;
                _myPM.frictionCombine = PhysicMaterialCombine.Maximum;
            }
            else
            {
                _myPM.dynamicFriction = 0.0f;
                _myPM.frictionCombine = PhysicMaterialCombine.Minimum;
            }
        }
        #endregion

    }
}
