using Codice.CM.Client.Differences;
using QBuild.Player.Controller;
using QBuild.Player.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Windows;

namespace QBuild.Player.State
{
    public class PlayerIdle : PlayerGroundState
    {
        public PlayerIdle(PlayerStateController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoCheck()
        {
            base.DoCheck();
        }

        public override void Enter()
        {
            base.Enter();

            //player.Movement?.SetVelocityZero();
            Vector3 set = new Vector3(0, player.Movement.GetNowVelocity().y, 0);
            player.Movement.SetVelocity(set);
            player.Movement.SetLockVelocity(true);
        }

        public override void Exit()
        {
            base.Exit();
            player.Movement.SetLockVelocity(false);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (xInput != 0 || zInput != 0)
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
}
