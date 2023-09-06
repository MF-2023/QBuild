using UnityEditor.Build;
using VContainer;

namespace QBuild.Mino
{
    public class MinoSpawnTick
    {
        [Inject]
        public MinoSpawnTick(MinoService minoService,MinoSpawnStop minoSpawnStop)
        {
            _minoService = minoService;
            _minoService.OnMinoPlaced += _ => NextFrameSpawnMino();
            minoSpawnStop.OnStop += stop => _isSpawned = stop;
        }
        
        public void Tick()
        {
            if (!_isSpawn)
            {
                return;
            }

            if (_isSpawned)
            {
                return;
            }
            _minoService.SpawnMino();
            _isSpawn = false;
        }

        public void NextFrameSpawnMino()
        {
            _isSpawn = true;
        }

        private bool _isSpawn = false;
        private bool _isSpawned = false;
        private readonly MinoService _minoService;
    }
}