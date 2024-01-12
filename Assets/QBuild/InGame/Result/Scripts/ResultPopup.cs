using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultPopup : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_animator == null)
            {
                Debug.LogError("Animatorが設定されていません", this);
            }
        }

        public async UniTask Show()
        {
            //_animator.Play("Open");
            await UniTask.WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f); 
        }

        public void Hide()
        {
            
        }
    }
}