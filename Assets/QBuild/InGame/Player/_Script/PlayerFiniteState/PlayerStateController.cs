using QBuild.Player.Core;
using QBuild.Player.State;
using System;
using UnityEngine;

namespace QBuild.Player.Controller
{
    public class PlayerStateController
    {
        private PlayerStateMachine _StateMachine;

        private PlayerIdle _IdleState;
        private PlayerMove _MoveState;
        private PlayerFall _FallState;
        private PlayerGoal _GoalState;

        private Core.Core _Core;
        private Movement _Movement;
        private Rotation _Rotation;
        private Vector3  _ClimbBlockPosition;

        public Vector3 ClimbBlockPosition { get { return _ClimbBlockPosition; } }
        public PlayerInputHandler inputHandler { get; private set; }
        public Core.Core Core { get { return _Core; } }
        public Movement Movement { get => _Movement ?? Core.GetCoreComponent(ref _Movement); }
        public Rotation Rotation { get => _Rotation ?? Core.GetCoreComponent(ref _Rotation); }
        public PlayerIdle IdleState { get { return _IdleState; } }
        public PlayerMove MoveState { get { return _MoveState; } }
        public PlayerFall FallState { get { return _FallState; } }

        public event Action<string, bool>   OnChangeAnimation;
        public event Action<Vector3>        OnSetPosition;
        public event Func<Vector3>          OnGetPlayerPos;
        public event Func<bool>             OnCheckBlock;

        public delegate bool CheckCanClimb(ref Vector3 pos);
        public event CheckCanClimb          OnCheckCanClimbBlock;



        public PlayerStateController(Core.Core core, PlayerInputHandler playerInputHandler, PlayerData data)
        {
            _Core = core;
            this.inputHandler = playerInputHandler;
            _StateMachine = new PlayerStateMachine();

            //各種ステータスの生成
            _IdleState = new PlayerIdle(this, _StateMachine, data, "idle");
            _MoveState = new PlayerMove(this, _StateMachine, data, "move");
            _FallState = new PlayerFall(this, _StateMachine, data, "fall");
            _GoalState = new PlayerGoal(this, _StateMachine, data, "goal");

            _StateMachine.Initialize(_IdleState);
        }

        public void LogicUpdate()
        {
            _StateMachine.currentState.LogicUpdate();
        }

        public void FixedUpdate()
        {
            _StateMachine.currentState.PhycsUpdate();
        }



        public void AnimationTrigger() => _StateMachine.currentState.AnimationTrigger();

        public void AnimationFinishedTrigger() => _StateMachine.currentState.AnimationFinishedTrigger();

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

        public bool CheckCanCrimbBlock()
        {
            bool ret = OnCheckCanClimbBlock != null ? OnCheckCanClimbBlock(ref _ClimbBlockPosition) : false;            
            return ret;
        }

        public void SetPosition(Vector3 pos) { OnSetPosition?.Invoke(pos); }

        public void ChangeGoalState()
        {
            _StateMachine.ChangeState(_GoalState);
        }
    }
}