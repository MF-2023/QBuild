using QBuild.Player.Core;
using QBuild.Player.State;
using System;
using UnityEngine;

namespace QBuild.Player.Controller
{
    public class PlayerStateController
    {
        private PlayerStateMachine stateMachine;

        private PlayerIdle _IdleState;
        private PlayerMove _MoveState;
        private PlayerFall _FallState;
        public PlayerIdle IdleState { get { return _IdleState; } }
        public PlayerMove MoveState { get { return _MoveState; } }
        public PlayerFall FallState { get { return _FallState; } }

        private Core.Core _Core;
        private Movement movement;
        private Rotation rotation;
        public PlayerInputHandler inputHandler { get; private set; }

        public Core.Core Core { get { return _Core; } }
        public Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
        public Rotation Rotation { get => rotation ?? Core.GetCoreComponent(ref rotation); }

        public event Action<string, bool> OnChangeAnimation;
        public event Func<Vector3> OnGetPlayerPos;
        public event Func<bool> OnCheckBlock;


        public PlayerStateController(Core.Core core, PlayerInputHandler playerInputHandler, PlayerData data)
        {
            _Core = core;
            this.inputHandler = playerInputHandler;
            stateMachine = new PlayerStateMachine();

            //各種ステータスの生成
            _IdleState = new PlayerIdle(this, stateMachine, data, "idle");
            _MoveState = new PlayerMove(this, stateMachine, data, "move");
            _FallState = new PlayerFall(this, stateMachine, data, "fall");

            stateMachine.Initialize(_IdleState);
        }

        public void LogicUpdate()
        {
            stateMachine.currentState.LogicUpdate();
        }

        public void FixedUpdate()
        {
            stateMachine.currentState.PhycsUpdate();
        }



        public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();

        public void AnimationFinishedTrigger() => stateMachine.currentState.AnimationFinishedTrigger();

        public void ChangeAnimationDelegateEvent(string animName, bool setBool)
        {
            if (OnChangeAnimation != null)
            {
                OnChangeAnimation(animName, setBool);
            }
        }

        public Vector3 GetPlayerPos()
        {
            Vector3 ret = Vector3.zero;

            if (OnGetPlayerPos != null)
            {
                ret = OnGetPlayerPos();
            }

            return ret;
        }

        public bool CheckBlock()
        {
            bool ret = false;
            if (OnCheckBlock != null) ret = OnCheckBlock();
            return ret;
        }
    }
}