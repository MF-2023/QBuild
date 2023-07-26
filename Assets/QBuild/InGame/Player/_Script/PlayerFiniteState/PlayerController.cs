using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region States
    private PlayerStateMachine stateMachine;

    public PlayerIdle IdleState { get; private set; }
    public PlayerMove MoveState { get; private set; }
    #endregion

    #region UnityComponent
    public Rigidbody _Rb { get; private set; }
    public Animator _Anim { get;private set; }
    #endregion

    #region Variables
    [SerializeField] private PlayerData playerData;
    public Core Core { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }


    public Movement Movement { get => movement ?? Core.GetCoreComponent(ref movement); }
    public Rotation Rotation { get => rotation ?? Core.GetCoreComponent(ref rotation); }
    private Movement movement;
    private Rotation rotation;
    #endregion

    #region UnityCallBack
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdle(this, stateMachine, playerData, "idle");
        MoveState = new PlayerMove(this, stateMachine, playerData, "move");
    }

    private void Start()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<PlayerInputHandler>();
        Core = GetComponentInChildren<Core>();

        stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        stateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.PhycsUpdate();
    }
    #endregion

    #region OtherFunction
    /// <summary>
    /// アニメーション用トリガー
    /// </summary>
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    /// <summary>
    /// アニメーション終了トリガー
    /// </summary>
    public void AnimationFinishedTrigger() => stateMachine.currentState.AnimationFinishedTrigger();
    #endregion
}
