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

    private Vector3 currentPosition;

    public event Action<Vector3> OnChangeGridPosition;
    #endregion

    #region UnityCallBack
    private void Start()
    {
        Core = GetComponentInChildren<Core>();
        _AnimationController = new PlayerAnimationController(GetComponent<Animator>());
        _StateController = new PlayerStateController(Core, _inputHandler, groundCheck, playerData);
        _StateController.OnChangeAnimation += _AnimationController.ChangeAnimation;
        _StateController.OnGetPlayerPos += GetPlayerPosition;
        currentPosition = transform.position;
        currentPosition = new Vector3((int)currentPosition.x, (int)currentPosition.y, (int)currentPosition.z);
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

    private void OnDrawGizmos()
    {
        //地面チェック用Ray
        Gizmos.DrawSphere(groundCheck.position, playerData.groundCheckRadius);
    }
    #endregion

    private Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public void AnimationTrigger() => _StateController.AnimationTrigger();

    public void AnimationFinishedTrigger() => _StateController.AnimationFinishedTrigger();

    private void CheckGridPosition()
    {
        Vector3 nowPos = transform.position;
        nowPos = new Vector3((int)nowPos.x, (int)nowPos.y, (int)nowPos.z);

        if(nowPos != currentPosition)
        {
            //グリッド変更のイベント呼び出し
            if (OnChangeGridPosition != null) OnChangeGridPosition(nowPos);
            currentPosition = nowPos;
        }
    }
}
