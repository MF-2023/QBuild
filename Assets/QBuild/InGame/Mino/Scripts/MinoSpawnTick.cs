using UnityEditor.Build;
using VContainer;

namespace QBuild.Mino
{
    public class MinoSpawnTick
    {
        [Inject]
        public MinoSpawnTick(MinoService minoService)
        {
            _minoService = minoService;
            _minoService.OnMinoPlaced += _ => NextFrameSpawnMino();
        }
        
        public void Tick()
        {
            if (!_isSpawn)
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
        private readonly MinoService _minoService;
    }
}