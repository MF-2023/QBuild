using System;
using UnityEngine;

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
            _StateController.OnCheckBlock += CheckGround;
            currentPosition = GetPlayerGridPosition();
        }

        private void Update()
        {
            _StateController.LogicUpdate();
            CheckGridPosition();

            //テスト
            //前方のポジションにブロックが存在するか？
            bool flg = true;
            Vector3Int check = new Vector3Int((int)(transform.position.x + playerData.checkBlockDistance), (int)transform.position.y - 1, (int)(transform.position.z + playerData.checkBlockDistance));
            if (OnCheckBlock != null ? OnCheckBlock(check) : false)
            {
                //その上にブロックは存在するか？
                check.y += 1;
                if (!(OnCheckBlock != null ? OnCheckBlock(check) : false))
                {
                    //存在しない場合Trueを返す
                    UnityEngine.Debug.LogWarning("登れる！！");
                    flg = false;
                }
            }

            if (flg) UnityEngine.Debug.LogWarning("登れない");
        }

        private void FixedUpdate()
        {
            _StateController.FixedUpdate();
        }

        private void OnDrawGizmosSelected()
        {
            Vector3Int check = new Vector3Int((int)(transform.position.x + playerData.checkBlockDistance),
                                              (int)transform.position.y,
                                              (int)(transform.position.z + playerData.checkBlockDistance));
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

        private bool CheckCanClimbBlock()
        {
            bool ret = false;
            //前方のポジションにブロックが存在するか？
            Vector3Int check = new Vector3Int((int)(transform.position.x + playerData.checkBlockDistance), (int)transform.position.y, (int)(transform.position.z + playerData.checkBlockDistance));
            if(OnCheckBlock != null ? OnCheckBlock(check) : false)
            {
                //その上にブロックは存在するか？
                check.y += 1;
                if(!(OnCheckBlock != null ? OnCheckBlock(check) : false))
                {
                    //存在しない場合Trueを返す
                    ret = true;
                }
            }
            return ret;
        }
    }
}
