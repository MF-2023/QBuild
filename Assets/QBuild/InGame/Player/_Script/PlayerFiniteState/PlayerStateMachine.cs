using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.State
{
    public class PlayerStateMachine
    {
        public PlayerState currentState { get; private set; }
        public PlayerState oldCurrentState { get; private set; }

        /// <summary>
        /// StateMachineの初期化処理
        /// </summary>
        /// <param name="initState">初めにセットされる状態</param>
        public void Initialize(PlayerState initState)
        {
            currentState = initState;
            oldCurrentState = initState;

            currentState.Enter();
        }

        /// <summary>
        /// 現在の状態の変更
        /// </summary>
        /// <param name="changeState">変更する状態</param>
        public void ChangeState(PlayerState changeState)
        {
            oldCurrentState = currentState;
            currentState.Exit();
            currentState = changeState;
            currentState.Enter();
        }
    }
}
