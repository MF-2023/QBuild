using System;
using QBuild.Player.Core;
using SherbetInspector.Core.Attributes;
using UnityEngine;

namespace QBuild
{
    public class HealthBarDemo : MonoBehaviour
    {
        private void Start()
        {
            Debug.LogWarning("体力バーのデモ版を利用", this);
            var healthBar = GetComponent<HealthBar>();
            healthBar.Initialize(5, 5);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DecreaseHealth();
            }
        }
        
        [Button("ハートを減らす(UI上のみ)")]
        private void DecreaseHealth()
        {
            var healthBar = GetComponent<HealthBar>();
            healthBar.UpdateHealth(healthBar.CurrentHealth - 1);
        }
    }
}