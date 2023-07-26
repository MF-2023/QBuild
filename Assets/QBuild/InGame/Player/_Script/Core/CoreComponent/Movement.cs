using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    private Vector3 workspace;
    private Vector3 currentVelocity;
    private bool canSetVelocity;
    private Rigidbody myRB;

    protected override void Start()
    {
        base.Start();
        myRB = GetComponentInParent<Rigidbody>();
        if (myRB == null) Debug.LogError(transform.root.name + "Ç…RigidBodyÇ™ë∂ç›ÇµÇ‹ÇπÇÒÅB");
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

    public void SetVelocity(Vector3 velocity,float speed)
    {
        workspace = velocity.normalized * speed;
        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        if (!canSetVelocity) return;
        myRB.velocity = workspace;
        currentVelocity = workspace;
    }

    private void AddForceFinalVelocity()
    {
        if (!canSetVelocity) return;
        myRB.AddForce(workspace);
        currentVelocity = workspace;
    }
    #endregion
}
