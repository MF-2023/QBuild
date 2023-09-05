using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.Core
{
    public class Rotation : CoreComponent
    {
        public bool canRotation { get; private set; }

        private Transform rootTransform;
        private Vector3 workspace;
        private Vector3 currentDir;

        protected override void Awake()
        {
            base.Awake();
            rootTransform = transform.root.GetComponent<Transform>();
            workspace = Vector3.zero;
            canRotation = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            currentDir = rootTransform.forward;
        }

        #region Set Function
        public void SetRotation(Vector3 direction)
        {
            workspace = direction;
            SetFinalDirection();
        }

        private void SetFinalDirection()
        {
            if (!canRotation) return;
            rootTransform.LookAt(workspace);
        }
        #endregion
    }
}
