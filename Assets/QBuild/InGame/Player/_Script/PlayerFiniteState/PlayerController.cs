using System;
using UnityEngine;

namespace QBuild.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables        
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerInputHandler _inputHandler;

        [Header("Player Debug Info"),Tooltip("�v���C���[�̃f�o�b�O�p�̕ϐ�")]
        [SerializeField] private bool playerDebug;
        [SerializeField] private Transform groundCheck;

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
            _StateController.OnGetPlayerPos += GetPlayerPosition;
            _StateController.OnCheckBlock += CheckGround;
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
        private Vector3 GetPlayerPosition()
        {
            return transform.position;
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
            bool ret = false;
            //?��v?��?��?��C?��?��?��[?��̃|?��W?��V?��?��?��?��?��̈��?��?��?��w?��?��
            Vector3Int check = new Vector3Int(currentPosition.x, currentPosition.y - 1, currentPosition.z);
            if (OnCheckBlock != null) ret = OnCheckBlock(check);
            else if(playerDebug) ret = DebugCheckGround();
            return ret;
        }

        #region DebugFunction
        private bool DebugCheckGround()
        {
            bool ret = false;

            return ret;
        }
        #endregion
    }
}
