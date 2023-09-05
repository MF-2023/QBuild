using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.State
{
    public class PlayerState
    {
        protected Controller.PlayerStateController player;
        protected PlayerStateMachine stateMachine;
        protected PlayerData playerData;

        protected string animBoolName;

        protected bool isExit;
        protected bool animationFinishedTrigger;
        protected bool isGrounded;

        //各種入力変数
        protected float xInput;
        protected float zInput;

        protected Vector3 workspace;

        public PlayerState(Controller.PlayerStateController player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        {
            this.player = player;
            this.stateMachine = stateMachine;
            this.playerData = playerData;
            this.animBoolName = animBoolName;

            xInput = 0f;
            zInput = 0f;
        }

        /// <summary>
        /// 状態に遷移したときに初めに行われる処理
        /// </summary>
        public virtual void Enter()
        {
            DoCheck();
            player.ChangeAnimationDelegateEvent(animBoolName, true);
            animationFinishedTrigger = false;
            isExit = false;
        }

        /// <summary>
        /// 状態が終了したときに行われる処理
        /// </summary>
        public virtual void Exit()
        {
            player.ChangeAnimationDelegateEvent(animBoolName, false);
        }

        /// <summary>
        /// 状態に遷移したときにチェックする処理
        /// </summary>
        public virtual void DoCheck() { isGrounded = player.CheckBlock(); }

        /// <summary>
        /// 現在の状態のアップデート処理
        /// </summary>
        public virtual void LogicUpdate()
        {
            xInput = player.inputHandler.xInput;
            zInput = player.inputHandler.zInput;
            isGrounded = player.CheckBlock();
        }

        /// <summary>
        /// 現在の状態の物理アップデート処理
        /// </summary>
        public virtual void PhycsUpdate() { }

        /// <summary>
        /// アニメーション用トリガー
        /// </summary>
        public virtual void AnimationTrigger() { }

        /// <summary>
        /// アニメーション終了トリガー
        /// </summary>
        public void AnimationFinishedTrigger() => animationFinishedTrigger = true;
    }
}
