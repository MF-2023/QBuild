using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{ 
    private PlayerStateMachine stateMachine;

    private PlayerIdle _IdleState;
    private PlayerMove _MoveState;
    private PlayerJump _JumpState;
    private PlayerFall _FallState;
    public PlayerIdle IdleState { get { return _IdleState; } }
    public PlayerMove MoveState { get { return _MoveState; } }
    public PlayerJump JumpState { get { return _JumpState;} }
    public PlayerFall FallState { get { return _FallState;} }

    private Transform _GroundCheck;
    private Core _Core;
    private Movement movement;
    private Rotation rotation;

    public Rigidbody _Rb { get; private set; }
    public Animator _Anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }

    public Transform GroundCheck { get { return _GroundCheck; } }
    public Core Core { get { return _Core; } }
    public Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
    public Rotation Rotation { get => rotation ?? Core.GetCoreComponent(ref rotation); }

    public void Initialize(PlayerData data , Transform groundCheck, PlayerInputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
        _Core = GetComponentInChildren<Core>();
        _GroundCheck = groundCheck;
        stateMachine = new PlayerStateMachine();
        _Rb = GetComponent<Rigidbody>();
        _Anim = GetComponent<Animator>();

        //各種ステータスの生成
        _IdleState = new PlayerIdle(this, stateMachine, data, "idle");
        _MoveState = new PlayerMove(this, stateMachine, data, "move");
        _JumpState = new PlayerJump(this, stateMachine, data, "jump");
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
}
