using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace QBuild.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables        
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerInputHandler _inputHandler;

        private PlayerStateController _StateController;
        private PlayerAnimationController _AnimationController;
        private Core.Core Core;

        private Vector3Int currentPosition;

        public event Action<Vector3> OnChangeGridPosition;
        public event Func<Vector3Int, bool> OnCheckBlock;
        #endregion

        #region UnityCallBack
        private void Start()
        {
            Core = GetComponentInChildren<Core.Core>();
            _AnimationController = new PlayerAnimationController(GetComponent<Animator>());
            _StateController = new PlayerStateController(Core, _inputHandler, playerData);
            _StateController.OnChangeAnimation += _AnimationController.ChangeAnimation;
            _StateController.OnGetPlayerPos += () => transform.position;
            _StateController.OnSetPosition += (Vector3 pos) => transform.position = pos;
            _StateController.OnCheckBlock += CheckGround;
            _StateController.OnCheckCanClimbBlock += CheckCanClimbBlock;
            currentPosition = GetPlayerGridPosition();
        }

        private void Update()
        {
            _StateController.LogicUpdate();
            CheckGridPosition();
        }

        private void FixedUpdate()
        {
            _StateController.FixedUpdate();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            float collectX = 0.0f;
            float collectZ = 0.0f;
            GetFrontBlockPos(ref collectX, ref collectZ);

            Vector3Int check = new Vector3Int((int)(transform.position.x + collectX),
                                              (int)transform.position.y + 1,
                                              (int)(transform.position.z + collectZ));
            Gizmos.DrawLine(transform.position, check);
        }
        #endregion

        private Vector3Int GetPlayerGridPosition()
        {
            //�؂�̂�
            //Vector3Int ret = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

            //�؂�グ�̂ق����悳���H
            Vector3Int ret = new Vector3Int(Mathf.CeilToInt(transform.position.x),
                                            Mathf.CeilToInt(transform.position.y),
                                            Mathf.CeilToInt(transform.position.z));

            return ret;
        }

        public void AnimationTrigger() => _StateController.AnimationTrigger();

        public void AnimationFinishedTrigger() => _StateController.AnimationFinishedTrigger();

        private void CheckGridPosition()
        {
            Vector3Int nowPos = GetPlayerGridPosition();

            if (nowPos != currentPosition)
            {
                //?��O?��?��?��b?��h?��ύX?��̃C?��x?��?��?��g?��Ăяo?��?��
                OnChangeGridPosition?.Invoke(nowPos);
                currentPosition = nowPos;
            }
        }

        private bool CheckGround()
        {          
            //�u���b�N�����݂���ꍇtrue��Ԃ�
            Vector3 pos = transform.position;
            Vector3Int check = new Vector3Int((int)(pos.x + 0.5f), currentPosition.y - 1, (int)(pos.z + 0.5f));
            if (OnCheckBlock != null ? OnCheckBlock(check) : false) return true;
            return false;
        }

        private bool CheckCanClimbBlock(ref Vector3 retPos)
        {
            bool ret = false;
            float collectX = 0.0f;
            float collectZ = 0.0f;
            GetFrontBlockPos(ref collectX, ref collectZ);

            Vector3Int check = new Vector3Int((int)(transform.position.x + collectX),
                                              (int)transform.position.y + 1,
                                              (int)(transform.position.z + collectZ));
            Vector3Int savePos = check;
            if (OnCheckBlock != null ? OnCheckBlock(check) : false)
            {
                //�v���C���[�̍������u���b�N�̊m�F������
                bool checkHeight = false;
                for(int i = 1;i <= playerData.playerHeight;i++)
                {
                    check.y += 1;
                    if((OnCheckBlock != null ? OnCheckBlock(check) : false))
                    {
                        checkHeight = true;
                    }
                }

                //�u���b�N�����݁i�o��Ȃ��ꍇ�j
                if (checkHeight) return false;

                retPos = savePos;
                retPos.y += 0.5f;
                return true;
            }
            return false;
        }

        private void GetFrontBlockPos(ref float collectX, ref float collectZ)
        {
            //�����Ƃ����������邩��
            float rot = transform.eulerAngles.y;
            rot = rot - (int)(rot / 360) * 360;
            rot = Mathf.Repeat(rot + 180f, 360f) - 180f;

            //���ʂ������Ă���ꍇ
            if (rot >= -45.0f && rot <= 45.0f)
            {
                if (transform.position.x >= 0) collectX = playerData.checkBlockCollectX;
                else collectX = playerData.checkBlockCollectX * -1.0f;
                if (transform.position.z >= 0) collectZ = playerData.checkBlockCollectZ;
                else collectZ = playerData.checkBlockCollectZ - 1.0f;
            }
            //�E�������Ă���ꍇ
            else if(rot >= 45.0f && rot <= 135.0f)
            {
                if (transform.position.x >= 0) collectX = playerData.checkBlockCollectZ;
                else collectX = playerData.checkBlockCollectZ - 1.0f;
                if (transform.position.z >= 0) collectZ = playerData.checkBlockCollectX;
                else collectZ = playerData.checkBlockCollectX * 1.0f;
            }
            //���������Ă���ꍇ
            else if(rot <= -45.0f && rot >= -135.0f)
            {
                if (transform.position.x >= 0) collectX = (1.0f - playerData.checkBlockCollectZ);
                else collectX = playerData.checkBlockCollectZ * -1.0f;
                if (transform.position.z >= 0) collectZ = playerData.checkBlockCollectX;
                else collectZ = playerData.checkBlockCollectX * 1.0f;
            }
            //���������Ă���ꍇ
            else
            {
                if (transform.position.x >= 0) collectX = playerData.checkBlockCollectX;
                else collectX = playerData.checkBlockCollectX * -1.0f;
                if (transform.position.z >= 0) collectZ = (1.0f - playerData.checkBlockCollectZ);
                else collectZ = playerData.checkBlockCollectZ * -1.0f;
            }
        }
    }
}
