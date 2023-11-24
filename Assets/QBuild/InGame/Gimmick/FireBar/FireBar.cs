using UnityEngine;

namespace QBuild.Gimmick
{
    public class FireBar : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            var contactPoint = other.GetContact(0);
            other.rigidbody.AddForce(-contactPoint.normal * 50f, ForceMode.Impulse);
        }
    }
}