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
            //切り捨て
            //Vector3Int ret = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

            //切り上げのほうがよさげ？
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
                //?ｿｽO?ｿｽ?ｿｽ?ｿｽb?ｿｽh?ｿｽﾏ更?ｿｽﾌイ?ｿｽx?ｿｽ?ｿｽ?ｿｽg?ｿｽﾄび出?ｿｽ?ｿｽ
                OnChangeGridPosition?.Invoke(nowPos);
                currentPosition = nowPos;
            }
        }

        private bool CheckGround()
        {
            bool ret = false;
            //?ｿｽv?ｿｽ?ｿｽ?ｿｽC?ｿｽ?ｿｽ?ｿｽ[?ｿｽﾌポ?ｿｽW?ｿｽV?ｿｽ?ｿｽ?ｿｽ?ｿｽ?ｿｽﾌ茨ｿｽﾂ会ｿｽ?ｿｽ?ｿｽ?ｿｽw?ｿｽ?ｿｽ
            Vector3Int check = new Vector3Int(currentPosition.x, currentPosition.y - 1, currentPosition.z);
            if (OnCheckBlock != null) ret = OnCheckBlock(check);
            return ret;
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
            if (OnCheckBlock != null ? OnCheckBlock(check) : false)
            {
                //その上にブロックは存在するか？
                check.y += 1;
                if(!(OnCheckBlock != null ? OnCheckBlock(check) : false))
                {
                    check.y += 1;
                    if (!(OnCheckBlock != null ? OnCheckBlock(check) : false))
                    {
                        //存在しない場合Trueを返す
                        ret = true;
                        check.y -= 2;
                        retPos = check;
                        //ブロックの半径分プラスしておく
                        retPos.y += 0.5f;
                    }
                }
            }
            return ret;
        }

        private void GetFrontBlockPos(ref float collectX, ref float collectZ)
        {
            //もっといいやり方あるかも
            float rot = transform.eulerAngles.y;
            rot = rot - (int)(rot / 360) * 360;
            rot = Mathf.Repeat(rot + 180f, 360f) - 180f;

            //正面を向いている場合
            if (rot >= -45.0f && rot <= 45.0f)
            {
                if (transform.position.x >= 0) collectX = playerData.checkBlockCollectX;
                else collectX = playerData.checkBlockCollectX * -1.0f;
                if (transform.position.z >= 0) collectZ = playerData.checkBlockCollectZ;
                else collectZ = playerData.checkBlockCollectZ - 1.0f;
            }
            //右を向いている場合
            else if(rot >= 45.0f && rot <= 135.0f)
            {
                if (transform.position.x >= 0) collectX = playerData.checkBlockCollectZ;
                else collectX = playerData.checkBlockCollectZ - 1.0f;
                if (transform.position.z >= 0) collectZ = playerData.checkBlockCollectX;
                else collectZ = playerData.checkBlockCollectX * 1.0f;
            }
            //左を向いている場合
            else if(rot <= -45.0f && rot >= -135.0f)
            {
                if (transform.position.x >= 0) collectX = (1.0f - playerData.checkBlockCollectZ);
                else collectX = playerData.checkBlockCollectZ * -1.0f;
                if (transform.position.z >= 0) collectZ = playerData.checkBlockCollectX;
                else collectZ = playerData.checkBlockCollectX * 1.0f;
            }
            //後ろを向いている場合
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
