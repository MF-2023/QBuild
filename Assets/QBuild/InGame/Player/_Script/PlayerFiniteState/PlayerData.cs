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
        public float inAirmoveSpeed = 2.5f;

        [Tooltip("�o���u���b�N�����m���钷��")] public float checkBlockDistance = 0.2f;
    }
}