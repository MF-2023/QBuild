using UnityEngine;

namespace QBuild.Gimmick
{
    public abstract class BaseGimmick : MonoBehaviour
    {
        public abstract void Active();
        public abstract void Disable();
        
        public bool CanInteractive => canInteractive;

        /// <summary>
        /// Activeの呼び出しを制御する。
        /// </summary>
        protected bool canInteractive = true;
    }
}