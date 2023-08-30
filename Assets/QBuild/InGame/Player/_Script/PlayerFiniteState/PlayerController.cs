using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private PlayerStateController _stateController;
    [SerializeField] private PlayerInputHandler _inputHandler;
    public PlayerStateController StateController { get { return _stateController; } }
    #endregion

    #region UnityCallBack
    private void Start()
    {
        _stateController.Initialize(playerData, groundCheck, _inputHandler);
    }

    private void Update()
    {
        _stateController.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateController.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        //地面チェック用Ray
        Gizmos.DrawSphere(groundCheck.position, playerData.groundCheckRadius);
    }
    #endregion
}
