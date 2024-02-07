using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace QBuild.Result
{
    public class ResultPopup : MonoBehaviour
    {
        //[SerializeField] private GameObject _popup;
        [SerializeField] private Animator _animator;
        //[SerializeField] private GameObject _gameClearImage;
        //[SerializeField] private GameObject _gameOverImage;

        [SerializeField] private List<RectTransform> _buttonRectTransforms;
        [SerializeField] private Selectable _firstButton;
        private bool _isClickAny;
        
        public bool IsClickAny {get{return _isClickAny; }}

        private void Awake()
        {
            if (_animator == null)
            {
                Debug.LogError("Animatorが設定されていません", this);
            }

            _isClickAny = false;
            /*
            _popup.SetActive(false);
            _gameClearImage.SetActive(false);
            _gameOverImage.SetActive(false);
            */
        }

        public async UniTask GoalShow(CancellationToken token)
        {
            //_popup.SetActive(true);
            //_gameClearImage.SetActive(true);
            //_gameOverImage.SetActive(false);
            _buttonRectTransforms.ForEach(t => t.localScale = Vector3.zero);

            _animator.Play("PopupClearOpen");
            await UniTask.WaitUntil( () => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f, cancellationToken: token);

            if (_firstButton != null) _firstButton.Select();
            
            var tasks = _buttonRectTransforms.Select(t => t.DOScale(1.0f, 0.5f).SetEase(Ease.OutQuart).ToUniTask(cancellationToken: token));
            await UniTask.WhenAll(tasks);
            
            _isClickAny = false;
        }
        
        public async UniTask FailedShow(CancellationToken token)
        {
            //_popup.SetActive(true);
            //_gameClearImage.SetActive(false);
            //_gameOverImage.SetActive(true);
            _animator.Play("PopupFaileOpen");
            await UniTask.WaitUntil( () => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f, cancellationToken: token);
            if (_firstButton != null) _firstButton.Select();

            _isClickAny = false;
        }

        public async UniTask Close(CancellationToken token)
        {
            _animator.Play("PopupClose");
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f,
                cancellationToken: token);
            //_popup.SetActive(false);
            //_gameClearImage.SetActive(false);
            //_gameOverImage.SetActive(false);
        }

        public void OnClickButton()
        {
            _isClickAny = true;
        }
    }
}