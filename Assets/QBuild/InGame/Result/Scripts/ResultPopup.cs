using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace QBuild.Result
{
    public class ResultPopup : MonoBehaviour
    {
        [SerializeField] private GameObject _popup;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _gameClearImage;
        [SerializeField] private Image _gameOverImage;

        private bool _isClickAny;
        
        public bool IsClickAny {get{return _isClickAny; }}

        private void Awake()
        {
            if (_animator == null)
            {
                Debug.LogError("Animatorが設定されていません", this);
            }

            _isClickAny = false;
            _popup.SetActive(false);
        }

        public async UniTask Show()
        {
            _popup.SetActive(true);
            
            //TODO::ゲームクリアかゲームオーバーかで表示を変える
            
            _animator.Play("PopupOpen");
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            _isClickAny = false;
        }

        public async UniTask Close()
        {
            _animator.Play("PopupClose");
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            _popup.SetActive(false);
        }

        public void OnClickButton()
        {
            _isClickAny = true;
        }
    }
}