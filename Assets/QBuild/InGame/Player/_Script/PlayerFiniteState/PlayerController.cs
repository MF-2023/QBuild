using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region States
    private PlayerStateMachine stateMachine;

    public PlayerIdle IdleState { get; private set; }
    #endregion

    #region UnityComponent
    public Rigidbody _Rb { get; private set; }
    public Animator _Anim { get;private set; }
    #endregion

    #region Variables
    [SerializeField] private PlayerData playerData;
    public Core Core { get; private set; }
    #endregion

    #region UnityCallBack
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdle(this, stateMachine, playerData, "idle");
    }

    private void Start()
    {
        _Anim = GetComponent<Animator>();
        _Rb = GetComponent<Rigidbody>();

        stateMachine.Initialize(IdleState);
        Core = GetComponentInChildren<Core>();
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
