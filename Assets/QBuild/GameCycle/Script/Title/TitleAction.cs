using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using QBuild.Scene;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace QBuild.GameCycle.Title
{
    public class TitleAction : MonoBehaviour
    {
        [Header("シーン管理")] 
        [SerializeField,Tooltip("ゲームセレクトのシーン番号")] private int _nextSceneIndex = 0;
        [SerializeField] private SceneChangeEffect _sceneChangeEffect = SceneChangeEffect.Fade;
        
        [Header("タイトルシーン管理")]
        [SerializeField] private GameObject _pressPushButtonText = null;
        [SerializeField] private GameObject _buttons = null;
        [SerializeField] private Selectable _firstButton = null;
        private List<Selectable> _homePanelSelectables = new();
        [SerializeField] private Popup _optionPopup = null;
        [SerializeField] private Popup _gameEndPopup = null;
        [SerializeField] private float _fadeTime = 0.5f;
        
        [SerializeField] private InputActionReference _inputActionReference = null;
        
        //TODO:[林] 絶対消す
        private bool _isStart = false;
        private void Start()
        {
            _homePanelSelectables.AddRange(_buttons.GetComponentsInChildren<Selectable>());
            if (_inputActionReference != null) _inputActionReference.action.performed += _ =>
            {
                if (_isStart) return;
                _isStart = true;
                ShowButtons();
            };
            ShowPressPushButtonText();
            _optionPopup.gameObject.SetActive(false);
            _gameEndPopup.gameObject.SetActive(false);
        }
        
        public void ShowButtons()
        {
            if(_pressPushButtonText != null)_pressPushButtonText.SetActive(false);
            
            _buttons.SetActive(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                _firstButton.Select();
            });
        }
        
        public void ShowPressPushButtonText()
        {
            _pressPushButtonText.SetActive(true);
            _buttons.SetActive(false);
        }
        
        public void ShowOptionPopup()
        {
            ShowOptionPopupAsync().Forget();
        }
        
        private async UniTask ShowOptionPopupAsync()
        {
            _optionPopup.gameObject.SetActive(true);

            var currentSelected = EventSystem.current.currentSelectedGameObject;
            _homePanelSelectables.ForEach(x => x.interactable = false);
            await _optionPopup.ShowPopupAsync();
            _homePanelSelectables.ForEach(x => x.interactable = true);
            EventSystem.current.SetSelectedGameObject(currentSelected);
        }

        public void ShowGameEndPopup()
        {
            ShowEndPopupAsync().Forget();
        }
        
        private async UniTask ShowEndPopupAsync()
        {
            _gameEndPopup.gameObject.SetActive(true);

            var currentSelected = EventSystem.current.currentSelectedGameObject;
            _homePanelSelectables.ForEach(x => x.interactable = false);
            await _gameEndPopup.ShowPopupAsync();
            _homePanelSelectables.ForEach(x => x.interactable = true);
            EventSystem.current.SetSelectedGameObject(currentSelected);
        }

        public void SceneChangeFadeOut()
        {
            SceneManager.ChangeSceneWait(_nextSceneIndex, _sceneChangeEffect, _fadeTime);
        }
    }
}
