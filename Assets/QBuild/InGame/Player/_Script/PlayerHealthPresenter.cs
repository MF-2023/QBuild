using System;
using QBuild.Player.Controller;
using VContainer;
using VContainer.Unity;

namespace QBuild.Player
{
    public class PlayerHealthPresenter : IInitializable,IDisposable
    {
        private readonly PlayerController _playerController;
        private readonly HealthBar _healthBar;
        
        [Inject]
        public PlayerHealthPresenter(PlayerController playerController,HealthBar healthBar)
        {
            _playerController = playerController;
            _healthBar = healthBar;
        }

        public void Initialize()
        {
            _healthBar.Initialize(_playerController.Health.GetMaxHealth(),_playerController.Health.GetNowHealth());
            _playerController.OnDamage += Damage;
        }

        public void Dispose()
        {
        }
        
        private void Damage()
        {
            _healthBar.UpdateHealth(_playerController.Health.GetNowHealth());
        }
    }
}