using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer;

namespace QBuild.Mino
{
    public class MinoFallTick
    {
        [Inject]
        public MinoFallTick(FallMino fallMino)
        {
            _fallMino = fallMino;
        }

        public async UniTask Start()
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.4));
                await _fallMino.Down(1);
                await UniTask.WaitWhile(() => _fallMino.IsBusy());
            }
        }

        private readonly FallMino _fallMino;
    }
}