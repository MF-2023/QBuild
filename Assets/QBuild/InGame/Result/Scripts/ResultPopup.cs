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

        public void Show()
        {
        }

        public void Hide()
        {
            
        }
    }
}