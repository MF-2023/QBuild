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
        _StateController.OnGetPlayerGridPos += GetPlayerGridPosition;
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
        //切り捨て
        //Vector3Int ret = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

        //切り上げのほうがよさげ？
        Vector3Int ret = new Vector3Int(Mathf.CeilToInt(transform.position.x),
                                        Mathf.CeilToInt(transform.position.y),
                                        Mathf.CeilToInt(transform.position.z));

        return ret;
    }
    private Vector3 GetPlayerPosition()
    {
        return transform.position;
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

        if(nowPos != currentPosition)
        {
            //�ｿｽO�ｿｽ�ｿｽ�ｿｽb�ｿｽh�ｿｽﾏ更�ｿｽﾌイ�ｿｽx�ｿｽ�ｿｽ�ｿｽg�ｿｽﾄび出�ｿｽ�ｿｽ
            OnChangeGridPosition?.Invoke(nowPos);
            currentPosition = nowPos;
        }
    }

    private bool CheckGround()
    {
        bool ret = false;
        //�ｿｽv�ｿｽ�ｿｽ�ｿｽC�ｿｽ�ｿｽ�ｿｽ[�ｿｽﾌポ�ｿｽW�ｿｽV�ｿｽ�ｿｽ�ｿｽ�ｿｽ�ｿｽﾌ茨ｿｽﾂ会ｿｽ�ｿｽ�ｿｽ�ｿｽw�ｿｽ�ｿｽ
        Vector3Int check = new Vector3Int(currentPosition.x, currentPosition.y - 1, currentPosition.z);
        if(OnCheckBlock != null) ret = OnCheckBlock(currentPosition);

        return ret;
    }
}
