using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using QBuild.Scene;
using UnityEngine.Serialization;

namespace QBuild.GameCycle
{
    public class FadeInOut : SceneChangerBase
    {
        [FormerlySerializedAs("_fadeObject")] [SerializeField] private GameObject _fadeCanvas = null;
        [SerializeField] Image _fadePanel = null;

        protected override void Initialize()
        {
            _sceneChangeEffect = SceneChangeEffect.Fade;
        }

        public override bool InSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (!base.InSCEffect(scTime, effect)) return false;
            FadeIn(scTime);

            return true;
        }
        
        public override bool OutSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (!base.OutSCEffect(scTime, effect)) return false;
            FadeOut(scTime);

            return true;
        }
        
        private void FadeIn(TweenCallback endEvent, float time)
        {
            _fadeCanvas.SetActive(true);
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                0f, // 最終的なalpha値
                time
            ).onComplete += () =>
            {
                endEvent();
                _fadeCanvas.SetActive(false);
            };
        }

        private void FadeOut(TweenCallback endEvent, float time)
        {
            _fadeCanvas.SetActive(true);
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                1f, // 最終的なalpha値
                time
            ).onComplete += () =>
            {
                endEvent();
                //_fadeCanvas.SetActive(false);
            };
        }
        
        private void FadeIn(float time)
        {
            _fadeCanvas.SetActive(true);
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                0f, // 最終的なalpha値
                time
            ).onComplete += () =>
            {
                _fadeCanvas.SetActive(false);
            };
        }
        
        private void FadeOut(float time)
        {
            _fadeCanvas.SetActive(true);
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                1f, // 最終的なalpha値
                time
            ).onComplete += () =>
            {
                //_fadeCanvas.SetActive(false);
            };
        }
    }
}
