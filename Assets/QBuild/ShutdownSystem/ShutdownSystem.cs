using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutdownSystem : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
