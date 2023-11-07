using UnityEngine;
using UnityEngine.Events;

namespace QBuild.Gimmick
{
    public class GimmickKey : BaseGimmick
    {
               
        [SerializeField,Tooltip("カギを取ったときのイベント")] private UnityEvent<bool> _onKey;
        
        public override void Active()
        {
            if ( canInteractive == false ) return;
            _onKey?.Invoke(true);
            this.gameObject.SetActive(false);
        }

        public override void Disable()
        {
          
        }
    }
}


