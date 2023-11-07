using UnityEngine;

namespace QBuild.Gimmick
{
    public abstract class BaseGimmick : MonoBehaviour
    {
        public abstract void Active();
        public abstract void Disable();
    }
}