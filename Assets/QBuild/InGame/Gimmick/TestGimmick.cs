using QBuild.Gimmick.Effector;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class TestGimmick : BaseGimmick, IEffector
    {
        public override void Active()
        {
            Debug.Log("Active");
        }

        public override void Disable()
        {
            Debug.Log("Disable");
        }
    }
}