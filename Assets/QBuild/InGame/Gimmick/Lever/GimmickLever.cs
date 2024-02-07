using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick
{
    public class GimmickLever : BaseGimmick
    {
        
        [SerializeField,Tooltip("レバーがアクティブになったときのイベント")]   private UnityEvent _ActiveLever;
        [SerializeField] private SimpleAnimator _animation;
        [SerializeField] private AudioSource _audioSource;
        public override void Active()
        {
            _animation.graph.Play();
            _audioSource.Play();
            _ActiveLever?.Invoke();
        }

        public override void Disable()
        {
        }
    }
}