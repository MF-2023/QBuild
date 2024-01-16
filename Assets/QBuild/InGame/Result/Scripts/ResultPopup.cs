using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultPopup : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private bool _isClickAny;
        
        public bool IsClickAny {get{return _isClickAny; }}

        private void Awake()
        {
            if (_animator == null)
            {
                Debug.LogError("Animatorが設定されていません", this);
            }

            _isClickAny = true;
        }

        private void Start()
        {
            TestUI();
        }

        private async UniTask TestUI()
        {
            await Show();
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            Close();
        }

        public async UniTask Show()
        {
            _animator.Play("PopupOpen");
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            _isClickAny = false;
        }

        public void Close()
        {
            _animator.Play("PopupClose");
        }

        public void OnClickSceenButton(int sceneIndex)
        {
            _isClickAny = true;
        }
    }
}