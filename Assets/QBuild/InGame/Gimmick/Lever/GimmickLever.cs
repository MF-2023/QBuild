using System;
using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick
{
    public class GimmickLever : BaseGimmick
    {
        
        [SerializeField,Tooltip("レバーがアクティブになったときのイベント")]   private UnityEvent _ActiveLever;
        [SerializeField] private SimpleAnimator _animation;
        [SerializeField] private AudioSource _audioSource;

        private bool _isActive;

        private void Awake()
        {
            _isActive = false;
        }

        public override void Active()
        {
            _animation.graph.Play();
            _ActiveLever?.Invoke();

            if (!_isActive)
            {
                _isActive = true;
                _audioSource.Play();
            }
        }

        public override void Disable()
        {
        }
    }
}