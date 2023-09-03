using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private PlayerInputHandler _inputHandler;
    private PlayerStateController _StateController;
    private PlayerAnimationController _AnimationController;
    private Core Core;

    private Vector3Int currentPosition;

    public event Action<Vector3> OnChangeGridPosition;
    public event Func<Vector3Int, bool> OnCheckBlock;
    #endregion

    #region UnityCallBack
    private void Start()
    {
        Core = GetComponentInChildren<Core>();
        _AnimationController = new PlayerAnimationController(GetComponent<Animator>());
        _StateController = new PlayerStateController(Core, _inputHandler, groundCheck, playerData);
        _StateController.OnChangeAnimation += _AnimationController.ChangeAnimation;
        _StateController.OnGetPlayerPos += GetPlayerPosition;
        _StateController.OnCheckBlock += CheckGround;
        currentPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
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

    private Vector3Int GetPlayerPosition()
    {
        Vector3Int ret = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        return ret;
    }

    public void AnimationTrigger() => _StateController.AnimationTrigger();

    public void AnimationFinishedTrigger() => _StateController.AnimationFinishedTrigger();

    private void CheckGridPosition()
    {
        Vector3Int nowPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        if(nowPos != currentPosition)
        {
            //グリッド変更のイベント呼び出し
            OnChangeGridPosition?.Invoke(nowPos);
            currentPosition = nowPos;
        }
    }

    private bool CheckGround()
    {
        bool ret = false;
        //プレイヤーのポジションの一つ下を指定
        Vector3Int check = new Vector3Int(currentPosition.x, currentPosition.y - 1, currentPosition.z);
        if(OnCheckBlock != null) ret = OnCheckBlock(currentPosition);

        return ret;
    }
}
