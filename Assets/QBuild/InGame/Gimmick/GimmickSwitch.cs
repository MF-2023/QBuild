using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick
{
    public class GimmickSwitch : BaseGimmick
    {
        [SerializeField,Tooltip("スイッチがアクティブになったときのイベント")]   private UnityEvent _ActiveSwithc;
        [SerializeField,Tooltip("スイッチが非アクティブになったときのイベント")] private UnityEvent _DisableSwitch;

        public override void Active()
        {
            _ActiveSwithc?.Invoke();
        }

        public override void Disable()
        {
            _DisableSwitch?.Invoke();
        }
    }
}