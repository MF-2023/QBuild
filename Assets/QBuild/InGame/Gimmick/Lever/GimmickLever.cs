using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick
{
    public class GimmickLever : BaseGimmick
    {
        
        [SerializeField,Tooltip("レバーがアクティブになったときのイベント")]   private UnityEvent _ActiveLever;
        
        public override void Active()
        {
            _ActiveLever?.Invoke();
        }

        public override void Disable()
        {
        }
    }
}