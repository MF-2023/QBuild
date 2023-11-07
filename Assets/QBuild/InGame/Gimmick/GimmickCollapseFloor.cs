using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick 
{
    public class GimmickCollapseFloor : BaseGimmick
    {
        [SerializeField,Tooltip("床に乗ってから落下するまでの時間")] private float _collapseTime;
        [SerializeField,Tooltip("崩れてから床が復活するまでの時間")] private float _recoveryTime;
        
        [SerializeField,Tooltip("床が落下したときのイベント")] private UnityEvent<bool> _onCollapseFloor;
        [SerializeField,Tooltip("床が復活したときのイベント")] private UnityEvent<bool> _onRecoveryFloor;

        
        public override void Active()
        {
           if ( canInteractive == false ) return;
           
           Invoke(nameof(OnCollapseFloor),_collapseTime);
           if ( _recoveryTime > 0)
            Invoke(nameof(OnRecoveryFloor),_collapseTime + _recoveryTime);
        }

        public override void Disable()
        {
          
        }
        
        private void OnCollapseFloor()
        {
            _onCollapseFloor?.Invoke(true);
            this.gameObject.SetActive(false);
        }
        
        private void OnRecoveryFloor()
        {
            _onRecoveryFloor?.Invoke(true);
            this.gameObject.SetActive(true);
        }
    }
}
