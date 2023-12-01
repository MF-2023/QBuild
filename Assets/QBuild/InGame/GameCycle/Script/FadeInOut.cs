using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace QBuild.GameCycle
{
    public class FadeInOut : MonoBehaviour
    {
        [SerializeField] Image _fadePanel = null;
        [SerializeField] float _fadeTime = 0.5f;

        public void FadeIn(TweenCallback endEvent)
        {
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                0f, // 最終的なalpha値
                _fadeTime
            ).onComplete = endEvent;
        }

        public void FadeOut(TweenCallback endEvent)
        {
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                1f, // 最終的なalpha値
                _fadeTime
            ).onComplete = endEvent;
        }
        
        public void FadeIn()
        {
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                0f, // 最終的なalpha値
                _fadeTime
            );
        }
        
        public void FadeOut()
        {
            DOTween.ToAlpha(
                () => _fadePanel.color,
                color => _fadePanel.color = color,
                1f, // 最終的なalpha値
                _fadeTime
            );
        }
    }
}
