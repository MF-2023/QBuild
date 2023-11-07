using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick
{
    public class Gimmick_Switch : BaseGimmick
    {
        [SerializeField,Tooltip("スイッチがアクティブになったときのイベント")]   private UnityEvent ActiveSwithc;
        [SerializeField,Tooltip("スイッチが非アクティブになったときのイベント")] private UnityEvent DisableSwitch;
        [SerializeField,Tooltip("スイッチの初期値")]                             private bool _InitSwitchEnable = false;

        private void Start()
        {
            if (_InitSwitchEnable) ActiveSwithc?.Invoke();
            else DisableSwitch?.Invoke();
        }

        public override void Active()
        {
            ActiveSwithc?.Invoke();
        }

        public override void Disable()
        {
            DisableSwitch?.Invoke();
        }
    }
}