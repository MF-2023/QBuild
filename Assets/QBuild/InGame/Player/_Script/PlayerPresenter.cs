using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Player
{
    public class PlayerPresenter : IInitializable
    {
        [Inject]
        public PlayerPresenter(PlayerController playerController,BlockStore blockStore)
        {
            _playerController = playerController;
            _blockStore = blockStore;
            Debug.Log("PlayerPresenter Inject");
        }
        public void Initialize()
        {
        }
        
        private readonly PlayerController _playerController;
        private readonly BlockStore _blockStore;

    }
}