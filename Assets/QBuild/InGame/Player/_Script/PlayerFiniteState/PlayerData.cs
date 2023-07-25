using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State"),Tooltip("プレイヤーの移動速度")]
    public float moveSpeed = 5.0f;
    //public float runSpeed = 8.0f;
}
