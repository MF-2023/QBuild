using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick
{
    public class GimmickLever : BaseGimmick
    {
        
        [SerializeField,Tooltip("レバーがアクティブになったときのイベント")]   private UnityEvent _ActiveLever;
        [SerializeField] private SimpleAnimator _animation;
        public override void Active()
        {
            _animation.graph.Play();
            _ActiveLever?.Invoke();
        }

        public override void Disable()
        {
        }
    }
}