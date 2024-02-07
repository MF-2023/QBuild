using System.Collections;
using System.Collections.Generic;
using QBuild.Player.Controller;
using QBuild.Player.State;
using UnityEngine;

namespace QBuild.Player
{
    public class PlayerDead : PlayerState
    {
        private bool endAnimation;
        
        public PlayerDead(PlayerStateController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            Vector3 set = new Vector3(0, player.Movement.GetNowVelocity().y, 0);
            player.Movement.SetVelocity(set);
            //player.Movement.SetLockVelocity(true);

            endAnimation = false;
        }

        public override void LogicUpdate()
        {
            Vector3 set = new Vector3(0, player.Movement.GetNowVelocity().y, 0);
            player.Movement.SetVelocity(set);
            
            if (animationFinishedTrigger && !endAnimation)
            {
                player.EndFailedAnimation();
                endAnimation = true;
            }
        }
    }
}
