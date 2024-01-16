using System.Collections;
using System.Collections.Generic;
using QBuild.Player.Controller;
using UnityEngine;

namespace QBuild.Player.State
{
    public class PlayerGoal : PlayerState
    {
        public PlayerGoal(PlayerStateController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            //ゴールアニメーションが終了したときの処理が必要なら
            if (animationFinishedTrigger)
            {
                
            }
        }
    }
}