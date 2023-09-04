using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    public PlayerInAirState(PlayerStateController player,PlayerStateMachine stateMachine,PlayerData playerData,string animBoolName):base(player,stateMachine,playerData,animBoolName)
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

        if (isGrounded &&  player.Movement.GetNowVelocity().y<= 0)
        {
            //TODO::PlayerInAirState::プレイヤーが地面についた時の処理
            //仮でとりあえずIdleStateに戻してます
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhycsUpdate()
    {
        base.PhycsUpdate();

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveForward = cameraForward * zInput + Camera.main.transform.right * xInput;

        Debug.Log($"cameraForward:{cameraForward} moveForward:{moveForward} right:{Camera.main.transform.right}");
        player.Movement?.SetVelocityXZ(moveForward, playerData.inAirmoveSpeed);

        Vector3Int pos = player.GetPlayerGridPos();
        workspace = new Vector3(moveForward.x + pos.x, moveForward.y + pos.y, moveForward.z + pos.z);
        player.Rotation?.SetRotation(workspace);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
}
