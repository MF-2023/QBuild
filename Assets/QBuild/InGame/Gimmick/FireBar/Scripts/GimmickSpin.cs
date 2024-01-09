using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickSpin : MonoBehaviour
    {
        [SerializeField] private float _spinSpeed;
        [SerializeField] private bool _isOn;

        private bool _isMoving;

        //回転方向
        private enum SpinDirection
        {
            Left,
            Right
        }

        [SerializeField] private SpinDirection _spinDirection;

        public void Active()
        {
            _isOn = true;
            OnActiveChanged();
        }

        public void Disable()
        {
            _isOn = false;
            OnActiveChanged();
        }

        private void SpinStart()
        {
            if(_isMoving) return;
            SpinAsync().Forget();
        }

        private async UniTask SpinAsync()
        {
            _isMoving = true;
            
            while (_isMoving || _isOn)
            {
                var lastAnglesY = transform.localRotation.eulerAngles.y;
                switch (_spinDirection)
                {
                    case SpinDirection.Left:
                        transform.Rotate(0, _spinSpeed * Time.deltaTime, 0);
                        break;
                    case SpinDirection.Right:
                        transform.Rotate(0, -_spinSpeed * Time.deltaTime, 0);
                        break;
                }
                var currentAnglesY = transform.localRotation.eulerAngles.y;
                if (!_isOn)
                {
                    //90度毎に回転を止める
                    if (Mathf.FloorToInt(lastAnglesY / 90) != Mathf.FloorToInt(currentAnglesY / 90))
                    {
                        _isMoving = false;
                    }
                }
                
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }

        private void Update()
        {
        }


        private void OnValidate()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                OnActiveChanged();
            }
#endif
        }

        private void OnActiveChanged()
        {
            if (_isOn)
            {
                if (!_isMoving) SpinStart();
            }
        }
    }
}