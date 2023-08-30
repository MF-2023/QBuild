using UnityEditor;
using UnityEngine;
using System;
using System.Collections;

namespace QBuild.Utilities
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public static class PlaymodeStateObserver
    {
        private static bool _wasPaused = false, _wasPlaying = false;

        public enum PlayModeStateChangedType
        {
            PressedPlayButton,
            Began,
            PressedEndButton,
            Ended,
            Paused,
            Resumed,
            ExecutedStep,
            Exception
        }

        public static event Action<PlayModeStateChangedType> OnChangedState = delegate { };

        public static event Action OnPressedPlayButton = delegate { };

        public static event Action OnBegan = delegate { };

        public static event Action OnPressedEndButton = delegate { };

        public static event Action OnEnded = delegate { };

        public static event Action OnPaused = delegate { };

        public static event Action OnResumed = delegate { };

        public static event Action OnExecutedStep = delegate { };

        static PlaymodeStateObserver()
        {
            EditorApplication.pauseStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PauseState pauseState)
        {
            var playModeStateChangedType = PlayModeStateChangedType.Exception;

            if (_wasPaused != EditorApplication.isPaused)
            {
                if (EditorApplication.isPaused)
                {
                    if (EditorApplication.isPlaying && !_wasPlaying)
                    {
                        OnBegan();
                    }

                    playModeStateChangedType = PlayModeStateChangedType.Paused;
                    OnPaused();
                }
                else
                {
                    playModeStateChangedType = PlayModeStateChangedType.Resumed;
                    OnResumed();
                }
            }
            else
                switch (EditorApplication.isPlaying)
                {
                    case false when EditorApplication.isPlayingOrWillChangePlaymode:
                        playModeStateChangedType = PlayModeStateChangedType.PressedPlayButton;
                        OnPressedPlayButton();
                        break;
                    case true when EditorApplication.isPlayingOrWillChangePlaymode:
                    {
                        if (EditorApplication.isPaused)
                        {
                            playModeStateChangedType = PlayModeStateChangedType.ExecutedStep;
                            OnExecutedStep();
                        }
                        else
                        {
                            playModeStateChangedType = PlayModeStateChangedType.Began;
                            OnBegan();
                        }

                        break;
                    }
                    case true when !EditorApplication.isPlayingOrWillChangePlaymode:
                        playModeStateChangedType = PlayModeStateChangedType.PressedEndButton;
                        OnPressedEndButton();
                        break;
                    case false when !EditorApplication.isPlayingOrWillChangePlaymode:
                        playModeStateChangedType = PlayModeStateChangedType.Ended;
                        OnEnded();
                        break;
                }

            OnChangedState(playModeStateChangedType);

            _wasPaused = EditorApplication.isPaused;
            _wasPlaying = EditorApplication.isPlaying;
        }
    }
#endif
}