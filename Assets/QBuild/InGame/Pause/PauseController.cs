using System;
using Cysharp.Threading.Tasks;
using QBuild.GameCycle;
using QBuild.Scene;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QBuild
{
    public class PauseController : MonoBehaviour,IDisposable
    {
        [SerializeField] private Popup _pausePopup;
        [SerializeField] private InputActionReference _pauseAction;

        [SerializeField] private InputController _playerInput;
        private void Start()
        {
            Debug.Log("PauseController Start");
            _pauseAction.action.Enable();
            _pauseAction.action.performed += InputPause;
        }
        
        public void Dispose()
        {
            _pauseAction.action.performed -= InputPause;
        }
        
        public void Pause()
        {
            UniTask.Create(PauseAsync);
        }
        
        private async UniTask PauseAsync()
        {
            _playerInput.SetUIActionMap();
            await _pausePopup.ShowPopupAsync();
            _playerInput.SetInGameActionMap();
        }
        
        public void ToStageSelect()
        {
            SceneManager.ChangeSceneWait(SceneBuildIndex.StageSelect);
        }


        private void InputPause(InputAction.CallbackContext ctx)
        {
            Debug.Log("PauseController InputPause");
            if(!_pausePopup.IsShow)
            {
                Pause();
            }
            else
            {
                _pausePopup.HidePopup();
            }
        }

    }
}