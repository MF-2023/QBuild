using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public float xInput { get;private set; }
    public float zInput { get; private set; }
    public bool jumpInput { get; private set; }

    private void Update()
    {
        //���C���v�b�g����
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
    }

    public void UseJumpInput() => jumpInput = false;
}
