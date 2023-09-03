using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace QBuild.Behavior
{
    /// <summary>
    /// 移動補間処理をスタックして、順番に実行するクラス
    /// </summary>
    public class GridMove : MonoBehaviour
    {
        
        public void MoveTo(Vector3 targetPosition)
        {
            _moveChannel.Writer.TryWrite(targetPosition);
        }
        
        private void Awake()
        {
            _moveChannel = Channel.CreateSingleConsumerUnbounded<Vector3>();

            var reader = _moveChannel.Reader;
            MoveToAsync(reader, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid MoveToAsync(ChannelReader<Vector3> targetPositionReader,
            CancellationToken cancellationToken)
        {
            await targetPositionReader.ReadAllAsync()
                .ForEachAsync(targetPosition => { MoveToAsync(targetPosition, _speed, cancellationToken).Forget(); },
                    cancellationToken);
        }

        private async UniTask MoveToAsync(Vector3 target, float speed, CancellationToken token)
        {
            // 目的地値に到達するまで無限ループする
            while (true)
            {
                var currentPosition = transform.position;
                var delta = target - currentPosition;
                var distance = delta.magnitude;

                if (distance < speed * Time.deltaTime)
                {
                    transform.position = target;
                    return;
                }
                else
                {
                    var direction = delta.normalized;
                    transform.position += direction * (speed * Time.deltaTime);
                    await UniTask.Yield(token);
                }
            }
        }

        [SerializeField] private float _speed = 1f;

        private Channel<Vector3> _moveChannel;
    }
}