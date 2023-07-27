using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRespawn : MonoBehaviour
{
    private void Update()
    {
        if(transform.position.y < -5.0f)
        {
            transform.position = new Vector3(0, 5, 0);
        }
    }
}
