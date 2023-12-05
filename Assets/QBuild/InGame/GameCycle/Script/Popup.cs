using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

namespace  QBuild.GameCycle
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private RectTransform _popupWindow = null;
        [SerializeField] private float _popupShowTime = 0.2f;
        
        public void ShowPopup()
        {
            _popupWindow.localScale = Vector3.zero;
            _popupWindow.DOScale(1.0f,_popupShowTime).SetEase(Ease.Linear);
        }

        public void HidePopup()
        {
            _popupWindow.DOScale(0.0f, _popupShowTime).SetEase(Ease.Linear)
                .OnComplete(() => this.gameObject.SetActive(false));
        }
    }
}
