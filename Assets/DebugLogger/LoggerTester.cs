using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Logger = QBuild.Logger.Logger;

public class LoggerTester : MonoBehaviour
{

    private TextMeshProUGUI _textMeshProUGUI;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Logger.logger.Log("TEST",Logger.LogTag.System,Logger.LogColor.red);
        }

    }

   
}
