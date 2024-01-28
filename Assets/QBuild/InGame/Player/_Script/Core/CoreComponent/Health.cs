using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Player.Core
{
    public class Health : CoreComponent
    {
        private Action _healthZeroEvent;
        private Action _damageEvent;
        private int _maxHealth;
        private int _currentHealth;
        
        public int GetNowHealth() => _currentHealth;

        public void Initialize(int maxHealth, Action damageEvent ,Action healthZeroEvent)
        {
            _healthZeroEvent += healthZeroEvent;
            _damageEvent += damageEvent;
            _maxHealth = maxHealth;
            _currentHealth = _maxHealth;
        }

        public void Damage(int damage)
        {
            _currentHealth -= damage;
            _damageEvent?.Invoke();
            
            if (_currentHealth <= 0)
                _healthZeroEvent?.Invoke();
        }

        public void Heal(int heal)
        {
            _currentHealth += heal;
        }

        public void ResetHealth()
        {
            _currentHealth = _maxHealth;
        }
    }
}
