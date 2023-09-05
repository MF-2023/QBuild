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
        /// StateMachine�̏���������
        /// </summary>
        /// <param name="initState">���߂ɃZ�b�g�������</param>
        public void Initialize(PlayerState initState)
        {
            currentState = initState;
            oldCurrentState = initState;

            currentState.Enter();
        }

        /// <summary>
        /// ���݂̏�Ԃ̕ύX
        /// </summary>
        /// <param name="changeState">�ύX������</param>
        public void ChangeState(PlayerState changeState)
        {
            oldCurrentState = currentState;
            currentState.Exit();
            currentState = changeState;
            currentState.Enter();
        }
    }
}
