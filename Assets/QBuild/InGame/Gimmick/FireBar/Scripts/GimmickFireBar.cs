using System;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickFireBar : BaseGimmick
    {
        [SerializeField] private bool _isOn;
        [SerializeField] private FireBar _fireBar;

        public override void Active()
        {
            _isOn = !_isOn;
            OnEnableChanged();
        }

        public override void Disable()
        {
        }

        private void OnValidate()
        {
            OnEnableChanged();
        }


        private void OnEnableChanged()
        {
            if (_fireBar == null) return;
            if (_isOn)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Hide()
        {
            _fireBar.gameObject.SetActive(false);
        }

        private void Show()
        {
            _fireBar.gameObject.SetActive(true);
        }
    }
}