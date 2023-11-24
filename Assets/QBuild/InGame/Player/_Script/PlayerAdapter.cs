using System;
using QBuild.Player.Controller;
using QBuild.Player.Core;
using UnityEngine;

namespace QBuild.Player
{
    public class PlayerAdapter : MonoBehaviour , IMover
    {
        private PlayerController _pc;
        private Vector3 _currentMoverVelo;
        private bool _onMover;
        private Movement _movement;

        private void Start()
        {
            _onMover = false;
            if (!TryGetComponent<PlayerController>(out _pc))
                UnityEngine.Debug.LogError($"{transform.name}Ç…PlayerControllerÇ™ë∂ç›ÇµÇ‹ÇπÇÒ");
            _pc.Core.GetCoreComponent<Movement>(ref _movement);
        }

        public void OnMoverEnter()
        {
            _onMover = true;
            _movement.OnMover = true;
        }

        public void OnMoverExit()
        {
            _onMover = false;
            _movement.OnMover = false;
        }
        
        public void SetMoverVelocity(Vector3 velocity)
        {
            _currentMoverVelo = velocity;
            _movement.CurrentMoverVelocity = _currentMoverVelo;
        }
    }
}
