using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace QBuild.Mino
{
    public class MinoEventLifeCycle : ITickable, IAsyncStartable
    {
        [Inject]
        public MinoEventLifeCycle(MinoFallTick minoFallTick, MinoDestroyTick minoDestroyTick,
            MinoSpawnTick minoSpawnTick)
        {
            _minoFallTick = minoFallTick;
            _minoDestroyTick = minoDestroyTick;
            _minoSpawnTick = minoSpawnTick;
        }

        public void Tick()
        {
            _minoDestroyTick.Tick();

            _minoSpawnTick.Tick();
        }


        private readonly MinoFallTick _minoFallTick;
        private readonly MinoDestroyTick _minoDestroyTick;
        private readonly MinoSpawnTick _minoSpawnTick;

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _minoFallTick.Start();
        }
    }
}