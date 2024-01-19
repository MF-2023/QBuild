using QBuild.Player.Core;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class FireBar : MonoBehaviour
    {
        [SerializeField] private float _knockbackPower = 25f;

        private void OnCollisionStay(Collision other)
        {
            var contactPoint = other.GetContact(0);
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.Knockback(-contactPoint.normal, _knockbackPower);
            }
        }
    }
}