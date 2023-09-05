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
        public PlayerPresenter(PlayerController playerController, BlockStore blockStore)
        {
            _playerController = playerController;
            _blockStore = blockStore;
        }

        public void Initialize()
        {
            _playerController.OnCheckBlock += CheckBlock;
        }
        public void Dispose()
        {
            _playerController.OnCheckBlock -= CheckBlock;
        }
        
        private bool CheckBlock(Vector3Int position)
        {
            return _blockStore.TryGetBlock(position, out _);
        }

        private readonly PlayerController _playerController;
        private readonly BlockStore _blockStore;
    }
}