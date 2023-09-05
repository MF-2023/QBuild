using QBuild.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Windows;

namespace QBuild.Player.State
{
    public class PlayerMove : PlayerGroundState
    {
        private bool collBlock = false;
        private float startCollBlock;

        public PlayerMove(PlayerStateController player, PlayerStateMachine stateMachine, PlayerData playerData,
            string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoCheck()
        {
            base.DoCheck();
        }

        public override void Enter()
        {
            base.Enter();
            collBlock = false;
            startCollBlock = 0.0f;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            /*
            TODO::�ьN::�v���C���[�̑O�Ƀu���b�N�����邩����
            if()
            {
                //�u���b�N�̏�ɏ��X�e�[�^�X�Ɉڍs
            }
            */

            //�o���u���b�N���O���ɂ��邩�`�F�b�N
            if(player.CheckCanCrimbBlock())
            {
                if(!collBlock)
                {
                    collBlock = true;
                    startCollBlock = Time.time;
                }
            }

            //���̃u���b�N��鏈��
            if(collBlock)
            {

            }
            if (xInput == 0 && zInput == 0)
                stateMachine.ChangeState(player.IdleState);
        }

        public override void PhycsUpdate()
        {
            base.PhycsUpdate();

            //�v���C���[�̌����������̌v�Z
            Vector3 cameraForward = Vector3.Scale(UnityEngine.Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 moveForward = cameraForward * zInput + UnityEngine.Camera.main.transform.right * xInput;
            player.Movement?.SetVelocityXZ(moveForward, playerData.moveSpeed);

            //�v���C���[�̌��������̌v�Z
            Vector3 pos = player.GetPlayerPos();
            workspace = new Vector3(moveForward.x + pos.x, moveForward.y + pos.y, moveForward.z + pos.z);
            player.Rotation?.SetRotation(workspace);
        }

        public override void AnimationTrigger()
        {
            base.AnimationTrigger();
        }
    }
}