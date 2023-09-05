using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace QBuild.Player
{
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Base State Variables"), Tooltip("プレイヤーの移動速度")]
        public float moveSpeed = 5.0f;
        //public float runSpeed = 8.0f;
        [Tooltip("プレイヤーの空中移動速度")]
        public float inAirmoveSpeed = 2.5f;

        [Tooltip("プレイヤーのジャンプ")]
        public float jumpPower = 1.0f;

        [Header("Player Debug"), Tooltip("プレイヤーのデバッグ用データ")]
        public LayerMask GroundLayer;
    }
}