using UnityEngine;

namespace QBuild.Player
{
    public class PlayerHealthAdapter : HealthAdapter
    {
        [SerializeField] private PlayerCurrentData _playerCurrentData;
        
        public override int CurrentHealth => _playerCurrentData.CurrentHelth;
    }
}