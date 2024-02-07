using System;
using UnityEngine;

namespace QBuild
{
    public class HealthBarPresenter : MonoBehaviour
    {
        [SerializeField] private HealthAdapter _healthAdapter;
        [SerializeField] private HealthBar _healthBarView;

        private void Start()
        {
            //警告
            if (_healthAdapter == null)
            {
                Debug.LogError("体力アダプターが設定されていません", this);
            }

            if (_healthBarView == null)
            {
                Debug.LogError("体力バーが設定されていません", this);
            }
        }

        private void Update()
        {
            _healthBarView.UpdateHealth(_healthAdapter.CurrentHealth);
        }
    }
}