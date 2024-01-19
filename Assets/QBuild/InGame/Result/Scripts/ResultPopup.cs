using System;
using System.Threading;
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
        [SerializeField] private GameObject _gameClearImage;
        [SerializeField] private GameObject _gameOverImage;

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
            _gameClearImage.SetActive(false);
            _gameOverImage.SetActive(false);
        }

        public async UniTask Show(CancellationToken token)
        {
            _popup.SetActive(true);
            
            //TODO::ゲームクリアかゲームオーバーかで表示を変える
            
            _animator.Play("PopupOpen");
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f, cancellationToken: token);
            _isClickAny = false;
        }

        public async UniTask GoalShow(CancellationToken token)
        {
            _popup.SetActive(true);
            _gameClearImage.SetActive(true);
            _gameOverImage.SetActive(false);
            
            _animator.Play("PopupOpen");
            await UniTask.WaitUntil( () => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f, cancellationToken: token);
            _isClickAny = false;
        }
        
        public async UniTask FailedShow(CancellationToken token)
        {
            _popup.SetActive(true);
            _gameClearImage.SetActive(false);
            _gameOverImage.SetActive(true);
            
            _animator.Play("PopupOpen");
            await UniTask.WaitUntil( () => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f, cancellationToken: token);
            _isClickAny = false;
        }

        public async UniTask Close(CancellationToken token)
        {
            _animator.Play("PopupClose");
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);
            _popup.SetActive(false);
            _gameClearImage.SetActive(false);
            _gameOverImage.SetActive(false);
        }

        public void OnClickButton()
        {
            _isClickAny = true;
        }
    }
}