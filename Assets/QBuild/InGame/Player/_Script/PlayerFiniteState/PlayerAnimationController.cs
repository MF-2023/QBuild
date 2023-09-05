using UnityEngine;

namespace QBuild.Player.Controller
{
    public class PlayerAnimationController
    {
        private Animator anim;

        public PlayerAnimationController(Animator anim)
        {
            this.anim = anim;
        }

        public void ChangeAnimation(string animName, bool setBool)
        {
            anim.SetBool(animName, setBool);
        }
    }
}
