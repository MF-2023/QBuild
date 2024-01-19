using System;
using QBuild.Player.Core;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

namespace QBuild.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables        
        [SerializeField] private PlayerData         _playerData;
        [SerializeField] private PlayerInputHandler _inputHandler;
        [SerializeField] private Transform          _GroundCheckPosition;
        [SerializeField] private float              _GroundCheckRadius = 0.5f;
        [SerializeField] private LayerMask          _GroundLayer;

        private PlayerStateController _StateController;
        private PlayerAnimationController _AnimationController;
        private Core.Core _core;
        private Movement _movement;
        public Core.Core Core
        {
            get { return _core; }
        }

        private Vector3Int currentPosition;
        public event Action<Vector3> OnChangeGridPosition;
        public event Func<Vector3Int, bool> OnCheckBlock;
        #endregion

        #region UnityCallBack

        private void Awake()
        {
            _core = GetComponentInChildren<Core.Core>();
        }

        private void Start()
        {
            _AnimationController = new PlayerAnimationController(GetComponent<Animator>());
            _StateController = new PlayerStateController(_core, _inputHandler, _playerData);
            _StateController.OnChangeAnimation += _AnimationController.ChangeAnimation;
            _StateController.OnGetPlayerPos += () => transform.position;
            _StateController.OnSetPosition += (Vector3 pos) => transform.position = pos;
            _StateController.OnCheckBlock += CheckGround;
            _StateController.OnCheckCanClimbBlock += CheckCanClimbBlock;
            currentPosition = GetPlayerGridPosition();

            TryGetComponent<Collider>(out Collider coll);
            _movement = _core.GetCoreComponent<Movement>();
            if(_movement != null && coll != null)
            {
                _movement.SetPhysicMaterial(coll.material);
            }
        }

        private void Update()
        {
            _core.CoreLogicUpdate();
            _StateController.LogicUpdate();
            CheckGridPosition();
        }

        private void FixedUpdate()
        {
            _core.CoreFixedUpdate();
            _StateController.FixedUpdate();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            float collectX = 0.0f;
            float collectZ = 0.0f;
            GetFrontBlockPos(ref collectX, ref collectZ);

            Vector3Int check = new Vector3Int((int)(transform.position.x + collectX),
                                              (int)transform.position.y + 1,
                                              (int)(transform.position.z + collectZ));
            Gizmos.DrawLine(transform.position, check);

            Color setColor = Color.green;
            setColor.a = 0.5f;
            Gizmos.color = setColor;
            //ポジションチェック表示
            Gizmos.DrawSphere(_GroundCheckPosition.position, _GroundCheckRadius);
        }
        #endregion

        private Vector3Int GetPlayerGridPosition()
        {
            Vector3 pos = transform.position;
            Vector3Int ret = new Vector3Int((int)(pos.x), Mathf.RoundToInt(transform.position.y), (int)(pos.z));
            return ret;
        }


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
            /*
            Vector3 pos = transform.position;
            Vector3Int check = new Vector3Int((int)(pos.x + 0.5f), currentPosition.y - 1, (int)(pos.z + 0.5f));
            if (OnCheckBlock != null ? OnCheckBlock(check) : false) return true;
            return false;
            */            
            return Physics.CheckSphere(_GroundCheckPosition.position, _GroundCheckRadius, _GroundLayer);
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
                for(int i = 1;i <= _playerData.playerHeight;i++)
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
                if (transform.position.x >= 0) collectX = _playerData.checkBlockCollectX;
                else collectX = _playerData.checkBlockCollectX * -1.0f;
                if (transform.position.z >= 0) collectZ = _playerData.checkBlockCollectZ;
                else collectZ = _playerData.checkBlockCollectZ - 1.0f;
            }
            //�E�������Ă���ꍇ
            else if(rot >= 45.0f && rot <= 135.0f)
            {
                if (transform.position.x >= 0) collectX = _playerData.checkBlockCollectZ;
                else collectX = _playerData.checkBlockCollectZ - 1.0f;
                if (transform.position.z >= 0) collectZ = _playerData.checkBlockCollectX;
                else collectZ = _playerData.checkBlockCollectX * 1.0f;
            }
            //���������Ă���ꍇ
            else if(rot <= -45.0f && rot >= -135.0f)
            {
                if (transform.position.x >= 0) collectX = (1.0f - _playerData.checkBlockCollectZ);
                else collectX = _playerData.checkBlockCollectZ * -1.0f;
                if (transform.position.z >= 0) collectZ = _playerData.checkBlockCollectX;
                else collectZ = _playerData.checkBlockCollectX * 1.0f;
            }
            //���������Ă���ꍇ
            else
            {
                if (transform.position.x >= 0) collectX = _playerData.checkBlockCollectX;
                else collectX = _playerData.checkBlockCollectX * -1.0f;
                if (transform.position.z >= 0) collectZ = (1.0f - _playerData.checkBlockCollectZ);
                else collectZ = _playerData.checkBlockCollectZ * -1.0f;
            }
        }
        public void AnimationTrigger() => _StateController.AnimationTrigger();

        public void AnimationFinishedTrigger() => _StateController.AnimationFinishedTrigger();

        public void Goal()
        {
            _StateController.ChangeGoalState();
        }

        public void SetIcon(Sprite icon) 
        {

        }
    }
}
