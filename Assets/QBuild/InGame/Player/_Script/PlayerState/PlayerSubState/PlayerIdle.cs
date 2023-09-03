using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerGroundState
{
    public PlayerIdle(PlayerStateController player,PlayerStateMachine stateMachine,PlayerData playerData,string animBoolName):base(player,stateMachine,playerData,animBoolName)
    {
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();

        player.Movement?.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (jumpInput)
        {
            player.inputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if (xInput != 0 || zInput != 0)
            stateMachine.ChangeState(player.MoveState);
    }

    public override void PhycsUpdate()
    {
        base.PhycsUpdate();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
}
