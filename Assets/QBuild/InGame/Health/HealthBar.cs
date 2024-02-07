using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    public class HealthBar : MonoBehaviour
    {
        private int _maxHealth;
        private int _currentHealth;
        private List<HealthBarElement> _healthIcons = new();
        [SerializeField] private HealthBarElement _healthBarElementPrefab;
        
        [SerializeField] private Sprite _fullHealthIcon;
        [SerializeField] private Sprite _emptyHealthIcon;
        
        public int CurrentHealth => _currentHealth;
        
        public void Initialize(int maxHealth, int currentHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            for (var i = 0; i < _maxHealth; i++)
            {
                var healthBarElement = Instantiate(_healthBarElementPrefab, transform);
                healthBarElement.SetIcon(i < _currentHealth ? _fullHealthIcon : _emptyHealthIcon);
                _healthIcons.Add(healthBarElement);
            }
        }
        
        public void UpdateHealth(int currentHealth)
        {
            _currentHealth = currentHealth;
            for (var i = 0; i < _maxHealth; i++)
            {
                _healthIcons[i].SetIcon(i < _currentHealth ? _fullHealthIcon : _emptyHealthIcon);
            }
        }
    }
}