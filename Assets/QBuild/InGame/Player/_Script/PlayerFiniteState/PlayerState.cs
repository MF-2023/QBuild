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
    /// ��ԂɑJ�ڂ����Ƃ��ɏ��߂ɍs���鏈��
    /// </summary>
    public virtual void Enter()
    {
        DoCheck();
        player._Anim.SetBool(animBoolName, true);
        animationFinishedTrigger = false;
    }

    /// <summary>
    /// ��Ԃ��I�������Ƃ��ɍs���鏈��
    /// </summary>
    public virtual void Exit() 
    { 
        player._Anim.SetBool(animBoolName, false);    
    }

    /// <summary>
    /// ��ԂɑJ�ڂ����Ƃ��Ƀ`�F�b�N���鏈��
    /// </summary>
    public virtual void DoCheck() { }

    /// <summary>
    /// ���݂̏�Ԃ̃A�b�v�f�[�g����
    /// </summary>
    public virtual void LogicUpdate() { }

    /// <summary>
    /// ���݂̏�Ԃ̕����A�b�v�f�[�g����
    /// </summary>
    public virtual void PhycsUpdate() { }

    /// <summary>
    /// �A�j���[�V�����p�g���K�[
    /// </summary>
    public virtual void AnimationTrigger() { }
    
    /// <summary>
    /// �A�j���[�V�����I���g���K�[
    /// </summary>
    public void AnimationFinishedTrigger() => animationFinishedTrigger = true;
}
