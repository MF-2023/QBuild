using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace  QBuild.GameCycle
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private Selectable _firstSelectable = null;
        [SerializeField] private RectTransform _popupWindow = null;
        [SerializeField] private float _popupShowTime = 0.2f;
        
        private bool _isShow = false;
        
        public void ShowPopup()
        {
            _popupWindow.localScale = Vector3.zero;
            _popupWindow.DOScale(1.0f,_popupShowTime).SetEase(Ease.Linear);
        }

        public async UniTask ShowPopupAsync()
        {
            _popupWindow.localScale = Vector3.zero;
            await _popupWindow.DOScale(1.0f,_popupShowTime).SetEase(Ease.Linear).AsyncWaitForCompletion();
            _isShow = true;
            EventSystem.current.SetSelectedGameObject(_firstSelectable.gameObject);
            await UniTask.WaitUntil(() => _isShow == false);
        }
        
        
        public void HidePopup()
        {
            _popupWindow.DOScale(0.0f, _popupShowTime).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    this.gameObject.SetActive(false);
                    _isShow = false;
                });
        }
    }
}
