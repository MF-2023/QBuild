using QBuild.Player.Controller;
using UnityEngine;

namespace QBuild.Stage
{
    public class GoalPoint : MonoBehaviour
    {
        
        private void Awake()
        {

        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController player))
            {
                
            }
        }
    }
}