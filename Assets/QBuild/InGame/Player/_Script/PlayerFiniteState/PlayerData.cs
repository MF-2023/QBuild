using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace QBuild.Player
{
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Base State Variables"), Tooltip("�v���C���[�̈ړ����x")]
        public float moveSpeed = 5.0f;
        //public float runSpeed = 8.0f;
        [Tooltip("�v���C���[�̋󒆈ړ����x")]
        public float inAirMoveSpeed = 2.5f;
        [Tooltip("�u���b�N�ɖ������ď��܂ł̎���")] public float crimbTime;

        [Header("Block Check Info")]
        [Tooltip("�o���u���b�N�����m����X���̕␳�l"),Range(-1.0f,1.0f)] public float checkBlockCollectX = 0.2f;
        [Tooltip("�o���u���b�N�����m����Z���̕␳�l"),Range(-1.0f,1.0f)] public float checkBlockCollectZ = 0.9f;
    }
}