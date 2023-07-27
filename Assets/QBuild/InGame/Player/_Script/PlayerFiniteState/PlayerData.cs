using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Base State Variables"),Tooltip("プレイヤーの移動速度")]
    public float moveSpeed = 5.0f;
    //public float runSpeed = 8.0f;
    [Tooltip("プレイヤーの空中移動速度")]
    public float inAirmoveSpeed = 2.5f;

    [Tooltip("プレイヤーのジャンプ")]
    public float jumpPower = 1.0f;

    [Header("Check Ground")]
    [Tooltip("地面判定になるオブジェクトのレイヤー")]
    public List<LayerMask> groundLayer = new List<LayerMask>();
    [Tooltip("プレイヤーから地面までの判定距離")]
    public float checkGroundDistance = 0.5f;
}
