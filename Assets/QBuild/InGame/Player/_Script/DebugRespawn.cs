using System;
using UnityEngine;

namespace QBuild.Player.Debug
{
    public class DebugRespawn : MonoBehaviour
    {
        [SerializeField] private Vector3 _position;
        
        private void Awake()
        {
            _position = transform.position;
        }

        private void Update()
        {
            if (transform.position.y < -5.0f)
            {
                transform.position = _position;
            }
        }
    }
}
