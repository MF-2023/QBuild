using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerGroundState
{
    public PlayerJump(PlayerStateController player,PlayerStateMachine stateMachine,PlayerData playerData,string animBoolName):base(player,stateMachine,playerData,animBoolName)
    { }

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

        if(animationFinishedTrigger)
        {
            workspace = new Vector3(player.Movement.GetNowVelocity().x, playerData.jumpPower, player.Movement.GetNowVelocity().z);
            player.Movement?.SetVelocity(workspace);
            stateMachine.ChangeState(player.FallState);
        }
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
