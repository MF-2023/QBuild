using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateController player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected string animBoolName;

    protected bool isExit;
    protected bool animationFinishedTrigger;
    protected bool isGrounded;

    //�e����͕ϐ�
    protected float xInput;
    protected float zInput;
    protected bool jumpInput;

    protected Vector3 workspace;

    public PlayerState(PlayerStateController player,PlayerStateMachine stateMachine,PlayerData playerData,string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;

        xInput = 0f;
        zInput = 0f;
        jumpInput = false;
    }

    /// <summary>
    /// ��ԂɑJ�ڂ����Ƃ��ɏ��߂ɍs���鏈��
    /// </summary>
    public virtual void Enter()
    {
        DoCheck();
        player.ChangeAnimationDelegateEvent(animBoolName, true);
        animationFinishedTrigger = false;
        isExit = false;
    }

    /// <summary>
    /// ��Ԃ��I�������Ƃ��ɍs���鏈��
    /// </summary>
    public virtual void Exit() 
    {
        player.ChangeAnimationDelegateEvent(animBoolName, false);
    }

    /// <summary>
    /// ��ԂɑJ�ڂ����Ƃ��Ƀ`�F�b�N���鏈��
    /// </summary>
    public virtual void DoCheck() { isGrounded = CheckIsGround(); }

    /// <summary>
    /// ���݂̏�Ԃ̃A�b�v�f�[�g����
    /// </summary>
    public virtual void LogicUpdate() 
    {
        xInput = player.inputHandler.xInput;
        zInput = player.inputHandler.zInput;
        jumpInput = player.inputHandler.jumpInput;
        isGrounded = CheckIsGround();
    }

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

    /// <summary>
    /// �n�ʂ̏�ɂ���̂����肷��
    /// </summary>
    /// <returns></returns>
    protected bool CheckIsGround()
    {
        Vector3 pos = player.GroundCheck.position;
        Collider[] hitColliders = Physics.OverlapSphere(pos, playerData.groundCheckRadius, playerData.groundLayer, QueryTriggerInteraction.Ignore);
        if (hitColliders.Length > 0)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                // �q�b�g�����I�u�W�F�N�g��Player�^�O�������Ă��Ȃ��ꍇ�̂ݏ������s��
                if (!hitCollider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
