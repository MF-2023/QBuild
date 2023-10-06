using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace QBuild.Logger
{
    public class Logger : MonoBehaviour
    {
        public enum LogColor
        {
            white,
            blue,
            red,
            green,
        }

        public enum LogTag
        {
            UI,
            System,
            Other,
        }

        public static Logger logger;

        private List<Log> _logs = new();

        private GameObject _logViewer;
        private TextMeshProUGUI _logTextUI;

        private const string _LoggerCanvasPath =
            "Assets/DebugLogger/LoggerCanvas.prefab";

        private const string _LoggerTextObjectPath =
            "Scroll View/Viewport/Content/LogText";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            new GameObject("LoggerCanvas", typeof(Logger));
        }

        private void Awake()
        {
            if (logger == null)
            {
                logger = this;
                DontDestroyOnLoad(gameObject);

                var viewer = AssetDatabase.LoadAssetAtPath<GameObject>(_LoggerCanvasPath);
                if (viewer == null)
                {
                    Debug.LogError("ViewerCanvasが見つかりませんでした。パスが正しく設定されているか確認してください。", this);
                    return;
                }

                _logViewer = Instantiate(viewer);

                var textObject = _logViewer.transform.Find(_LoggerTextObjectPath).gameObject;
                if (textObject == null)
                {
                    Debug.LogError("ログテキストが見つかりませんでした", this);
                    return;
                }

                if (!textObject.TryGetComponent(out TextMeshProUGUI obj))
                {
                    Debug.LogError("ログテキストのコンポーネントが見つかりませんでした", this);
                    return;
                }

                _logTextUI = obj;
            }
            else
            {
                Destroy(gameObject);
                Destroy(_logViewer);
            }

            _logViewer.SetActive(false);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _logViewer.SetActive(!_logViewer.activeSelf);     
                ViewLog();
            }
        }
        
        public void Log(
            string logText,
            LogTag logTag = LogTag.Other,
            LogColor logColor = LogColor.white,
            bool isOutputConsole = true
        )
        {
            DateTime now = DateTime.Now;
            var timeStamp = $"{now.Minute}:{now.Second}:{now.Millisecond}";

            Log log = new Log()
            {
                _logText = logText,
                _logTag = logTag,
                _textColor = logColor,
                _timeStamp = timeStamp,
                _stackTraceUtility = StackTraceUtility.ExtractStackTrace(),
            };

            _logs.Add(log);
            
            /*
            var log = $"<color={logColor}> [{logTag}] {logText} </color>";
            _logList.Add(log);

            var outputLog = $"{timeStamp}:[{logTag}] {logText}\n    >{StackTraceUtility.ExtractStackTrace()}";
            _outputLogList.Add(outputLog);
            */
            
            if ( _logViewer.activeSelf ) ViewLog();
            
            if (isOutputConsole) Debug.Log(log);
        }

        private void ViewLog()
        {
            _logTextUI.text = "";

            for (int i = _logs.Count - 1; i >= 0; i--)
            {
                _logTextUI.text +=
                    $"<color={_logs[i]._textColor}> [{_logs[i]._logTag}] {_logs[i]._logText} </color>\n";
            }
        }

        public List<Log> GetOutputLog()
        {
            return _logs;
        }
        
        public void ClearLog()
        {
            _logs.Clear();
            ViewLog();
        }
    }

    public class Log
    {
        public string _logText;
        public Logger.LogColor _textColor;
        public Logger.LogTag _logTag;
        public string _timeStamp;
        public string _stackTraceUtility;
    }
}