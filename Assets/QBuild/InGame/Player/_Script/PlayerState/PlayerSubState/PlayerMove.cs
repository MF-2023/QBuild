using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerGroundState
{
    public PlayerMove(PlayerStateController player, PlayerStateMachine stateMachine, PlayerData playerData,
        string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

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

        if (jumpInput)
        {
            player.inputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if (xInput == 0 && zInput == 0)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void PhycsUpdate()
    {
        base.PhycsUpdate();

        Vector3 cameraForward = Vector3.Scale(new Vector3(Camera.main.transform.forward.x, 0,Camera.main.transform.forward.z), new Vector3(1, 0, 1))
            .normalized;
        Vector3 moveForward = cameraForward * zInput + Camera.main.transform.right * xInput;
        
        
        player.Movement?.SetVelocityXZ(moveForward, playerData.moveSpeed);

        Vector3 pos = player.GetPlayerPos();
        workspace = new Vector3(moveForward.x + pos.x, moveForward.y + pos.y, moveForward.z + pos.z);
        player.Rotation?.SetRotation(workspace);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
}