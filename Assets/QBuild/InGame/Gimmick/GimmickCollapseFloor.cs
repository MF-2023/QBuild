using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick 
{
    public class GimmickCollapseFloor : BaseGimmick
    {
        [SerializeField,Tooltip("���ɏ���Ă��痎������܂ł̎���")] private float _collapseTime;
        [SerializeField,Tooltip("����Ă��珰����������܂ł̎���")] private float _recoveryTime;
        
        [SerializeField,Tooltip("�������������Ƃ��̃C�x���g")] private UnityEvent<bool> _onCollapseFloor;
        [SerializeField,Tooltip("�������������Ƃ��̃C�x���g")] private UnityEvent<bool> _onRecoveryFloor;

        
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
