using QBuild.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.State
{
    public class PlayerFall : PlayerInAirState
    {
        public PlayerFall(PlayerStateController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

