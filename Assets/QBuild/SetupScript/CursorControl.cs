using System;
using UnityEngine;

namespace QBuild.SetupScript
{
    public class CursorControl : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Cursor.visible = true;
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                Cursor.visible = false;
            }
        }
    }
}