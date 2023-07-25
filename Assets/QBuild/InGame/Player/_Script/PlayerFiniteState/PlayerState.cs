using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected string animBoolName;

    protected bool animationFinishedTrigger;

    public PlayerState(PlayerController player,PlayerStateMachine stateMachine,PlayerData playerData,string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    /// <summary>
    /// 状態に遷移したときに初めに行われる処理
    /// </summary>
    public virtual void Enter()
    {
        DoCheck();
        player._Anim.SetBool(animBoolName, true);
        animationFinishedTrigger = false;
    }

    /// <summary>
    /// 状態が終了したときに行われる処理
    /// </summary>
    public virtual void Exit() 
    { 
        player._Anim.SetBool(animBoolName, false);    
    }

    /// <summary>
    /// 状態に遷移したときにチェックする処理
    /// </summary>
    public virtual void DoCheck() { }

    /// <summary>
    /// 現在の状態のアップデート処理
    /// </summary>
    public virtual void LogicUpdate() { }

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
