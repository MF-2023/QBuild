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
        public float inAirMoveSpeed = 2.5f;
        [Tooltip("ブロックに密着して上るまでの時間")] public float crimbTime;

        [Header("Block Check Info")]
        [Tooltip("登れるブロックを検知するX軸の補正値"),Range(-1.0f,1.0f)] public float checkBlockCollectX = 0.2f;
        [Tooltip("登れるブロックを検知するZ軸の補正値"),Range(-1.0f,1.0f)] public float checkBlockCollectZ = 0.9f;
    }
}