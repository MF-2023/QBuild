using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Base State Variables"),Tooltip("�v���C���[�̈ړ����x")]
    public float moveSpeed = 5.0f;
    //public float runSpeed = 8.0f;
    [Tooltip("�v���C���[�̋󒆈ړ����x")]
    public float inAirmoveSpeed = 2.5f;

    [Tooltip("�v���C���[�̃W�����v")]
    public float jumpPower = 1.0f;

    [Header("Check Ground")]
    [Tooltip("�n�ʔ���ɂȂ�I�u�W�F�N�g�̃��C���[")]
    public List<LayerMask> groundLayer = new List<LayerMask>();
    [Tooltip("�v���C���[����n�ʂ܂ł̔��苗��")]
    public float checkGroundDistance = 0.5f;
}
