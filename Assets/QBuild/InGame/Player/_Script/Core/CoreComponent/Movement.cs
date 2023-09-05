using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.Core
{
    public class Movement : CoreComponent
    {
        public Vector3 currentVelocity { get; private set; }

        private Vector3 workspace;
        public bool canSetVelocity;
        private Rigidbody myRB;

        protected override void Awake()
        {
            base.Awake();
            myRB = GetComponentInParent<Rigidbody>();
            if (myRB == null) UnityEngine.Debug.LogError(transform.root.name + "‚ÉRigidBody‚ª‘¶Ý‚µ‚Ü‚¹‚ñB");
            workspace = Vector3.zero;
            canSetVelocity = true;
        }

        public override void LogicUpdate()
        {
            currentVelocity = myRB.velocity;
        }

        #region SetFunction
        public void SetVelocityZero()
        {
            workspace = Vector3.zero;
            SetFinalVelocity();
        }

        public void SetVelocity(Vector3 velocity, float speed)
        {
            workspace = velocity.normalized * speed;
            //workspace = new Vector3(velocity.x, 0, velocity.z).normalized * speed;
            SetFinalVelocity();
        }

        public void SetVelocity(Vector3 velocity)
        {
            workspace = velocity;
            SetFinalVelocity();
        }

        public void SetVelocityXZ(Vector3 velocity, float speed)
        {
            workspace = velocity.normalized * speed;
            workspace.y = myRB.velocity.y;
            SetFinalVelocity();
        }

        public void AddForce(Vector3 force, ForceMode mode)
        {
            workspace = force;
            AddForceFinalVelocity(mode);
        }

        public Vector3 GetNowVelocity()
        {
            return myRB.velocity;
        }

        private void SetFinalVelocity()
        {
            if (!canSetVelocity) return;
            myRB.velocity = workspace;
            currentVelocity = workspace;
        }

        private void AddForceFinalVelocity(ForceMode mode)
        {
            if (!canSetVelocity) return;
            myRB.AddForce(workspace, mode);
            currentVelocity = myRB.velocity;
        }

        public void SetCanVelocity(bool can) => canSetVelocity = can;
        #endregion
    }
}
