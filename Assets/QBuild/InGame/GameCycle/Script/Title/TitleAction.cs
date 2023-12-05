using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace QBuild.GameCycle.Title
{
    public class TitleAction : MonoBehaviour
    {
        [SerializeField] private GameObject _pressPushButtonText = null;
        [SerializeField] private GameObject _buttons = null;
        [SerializeField] private Popup _optionPopup = null;
        [SerializeField] private FadeInOut _fadeInOut = null;
        
        private void Start()
        {
            ShowPressPushButtonText();
            _optionPopup.gameObject.SetActive(false);
            FadeIn();
        }
        
        public void ShowButtons()
        {
            _pressPushButtonText.SetActive(false);
            _buttons.SetActive(true);
        }
        
        public void ShowPressPushButtonText()
        {
            _pressPushButtonText.SetActive(true);
            _buttons.SetActive(false);
        }
        
        public void ShowOptionPopup()
        {
            _optionPopup.gameObject.SetActive(true);
            _optionPopup.ShowPopup();
        }

        public void FadeIn()
        {
            _fadeInOut.gameObject.SetActive(true);
            _fadeInOut.FadeIn(() => _fadeInOut.gameObject.SetActive(false));
        }

        public void FadeOut()
        {
            _fadeInOut.gameObject.SetActive(true);
            _fadeInOut.FadeOut();
        }
    }
}
