using System.Threading;
using Cysharp.Threading.Tasks;
using QBuild.Player.Controller;
using QBuild.Player.Core;
using UnityEngine;

namespace QBuild.Player
{
    public class PlayerDamage : MonoBehaviour, IDamageable
    {
        private PlayerController _playerController;
        private Movement _movement;

        [SerializeField] private GameObject _mesh;
        //無敵
        [SerializeField] private bool _invincible = false;
        [SerializeField] private float _invincibleTime = 0.5f;
        
        [SerializeField] private float _flashInterval = 0.1f;
        [SerializeField] private float _flashTime = 0.1f;
        private float _invincibleTimer = 0f;

        private void Start()
        {
            if (!TryGetComponent<PlayerController>(out _playerController))
                UnityEngine.Debug.LogError($"{transform.name}にPlayerControllerが存在しません");
            _playerController.Core.GetCoreComponent<Movement>(ref _movement);
        }

        public void Knockback(Vector3 direction, float power, bool isForce = false)
        {
            if (_invincible) return;

            _movement.AddForce(direction * power, ForceMode.VelocityChange);
        }

        public void Damage(int damage, bool isForce = false)
        {
            if (_invincible)
            {
                if (isForce) _playerController.Health.Damage(damage);
                return;
            }

            _invincible = true;
            _invincibleTimer = 0f;
            UniTask.Create(InvincibleTimer);
            _playerController.Health.Damage(damage);
        }

        private async UniTask InvincibleTimer()
        {
            while (_invincibleTimer < _invincibleTime)
            {
                _invincibleTimer += Time.deltaTime;
                
                //点滅 _flashInterval秒間隔で_flashTime秒間点灯
                _mesh.SetActive(!(_invincibleTimer % _flashInterval < _flashTime));
                await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            }

            _mesh.SetActive(true);
            _invincible = false;
        }
    }
}