using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using QBuild.Player.Controller;

namespace QBuild.Player
{
    public class PlayerPresenter : IInitializable,IDisposable
    {
        [Inject]
        public PlayerPresenter(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Initialize()
        {
            _playerController.OnCheckBlock += CheckBlock;
        }
        public void Dispose()
        {
            _playerController.OnCheckBlock -= CheckBlock;
        }
        
        private bool CheckBlock(Vector3Int targetPosition)
        {
            var playerPosition = _playerController.transform.position;
            var direction = targetPosition - playerPosition;
            var results = new RaycastHit[1];
            var size = Physics.RaycastNonAlloc(playerPosition, direction, results, 1.0f, LayerMask.GetMask("Block"));
            if (size == 0)
            {
                return false;
            }
            return true;
        }

        private readonly PlayerController _playerController;
    }
}