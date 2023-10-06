using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using QBuild.Logger;
using Logger = QBuild.Logger.Logger;

public class LoggerModifier : MonoBehaviour
{

    [SerializeField] private string folderName;
    
    public void Save()
    {

        if (Logger.logger.GetOutputLog().Count == 0)
        {
            Debug.Log("ログがありません");
            return;
        }
        
        DateTime now = DateTime.Now;
       
        var fileName = $"QBuild-Log-{now.Month}-{now.Day}-{now.Hour}-{now.Minute}.txt";
        
        Directory.CreateDirectory(Application.persistentDataPath + "/" + folderName);
        string filePath = Path.Combine(Application.persistentDataPath + "/" + folderName, fileName);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Log item in Logger.logger.GetOutputLog())
            {
                var outputLog = $"{item._timeStamp}:[{item._logTag}] {item._logText}\n    >{item._stackTraceUtility}";
                writer.WriteLine(outputLog);
            }
        }

        Debug.Log("テキストファイルが作成されました: " + filePath);
    }
    
    public void Clear()
    {
        Logger.logger.ClearLog();

        Debug.Log("ログをクリアしました");
    }
}