using System;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace QBuild
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public static class EmergencyExit
    {
        static EmergencyExit()
        {
            Start();
        }

#if !UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod]
#endif
        private static void Start()
        {
            Application.logMessageReceived -= CatchThreadAbort;
            Application.logMessageReceived += CatchThreadAbort;

            ResetAbortThreadFlag();
            SpawnEmergencyThreadIfItDoesNotAlreadyExist();
        }

        private static void CatchThreadAbort(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception && condition == "ThreadAbortException")
            {
                ResetAbortThreadFlag();
            }
        }

        public static void ResetAbortThreadFlag()
        {
            if ((Thread.CurrentThread.ThreadState & (ThreadState.AbortRequested | ThreadState.Aborted)) == 0) return;
            Thread.ResetAbort();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }

        private static void SpawnEmergencyThreadIfItDoesNotAlreadyExist()
        {
            _mainThread ??= Thread.CurrentThread;

            if (_emergencyThread is { IsAlive: true }) return;
            _emergencyThread = new Thread(EmergencyTerminationThread)
            {
                Name = "Emergency Exit Thread"
            };
            _emergencyThread.Start();
        }


        private static void EmergencyTerminationThread()
        {
            while (true)
            {
                if (ShowEmergencyThreadActivity())
                    Debug.Log("emergencyThreadId " + Thread.CurrentThread.ManagedThreadId);

                if (EmergencyStopCode())
                {
                    try
                    {
                        _mainThread.Abort();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }

                Thread.Sleep(SleepTime);
            }
        }

#if UNITY_EDITOR_WIN
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int keycode);
#endif
        private static short GetKeyState(int keycode)
        {
#if UNITY_EDITOR_WIN
            return GetAsyncKeyState(keycode);
#else
            return 0;
#endif
        }
        
        private static bool EmergencyStopCode()
        {
            var shift = GetKeyState(0x10) < 0;
            var ctrl = GetKeyState(0x11) < 0;
            var q = GetKeyState(0x51) < 0;
            var e = GetKeyState(0x45) < 0;

            var pressed = shift && ctrl && q;
            var activate = pressed && !_lastEsc;
            _lastEsc = pressed;

            return activate || (shift && ctrl && e);
        }

        private static bool ShowEmergencyThreadActivity()
        {
            var shift = GetKeyState(0x10) < 0;
            var ctrl = GetKeyState(0x11) < 0;
            var h = GetKeyState(0x48) < 0;

            return shift && ctrl && h;
        }
        
        private const int SleepTime = 200;

        private static bool _lastEsc;

        private static Thread _mainThread;
        private static Thread _emergencyThread;
    }
}