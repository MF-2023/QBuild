using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerGroundState
{
    public PlayerMove(PlayerController player,PlayerStateMachine stateMachine,PlayerData playerData,string animBoolName):base(player,stateMachine,playerData,animBoolName)
    { }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput == 0 && zInput == 0)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void PhycsUpdate()
    {
        base.PhycsUpdate();

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * zInput + Camera.main.transform.right * xInput;

        player.Movement?.SetVelocity(moveForward, playerData.moveSpeed);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
}
