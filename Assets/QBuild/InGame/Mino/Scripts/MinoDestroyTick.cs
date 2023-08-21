using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace QBuild.Mino
{
    public class MinoDestroyTick
    {
        [Inject]
        public MinoDestroyTick(MinoService minoService)
        {
            _minoService = minoService;
        }
        
        public void Tick()
        {
            if (_dropMinos.Count ==0)
            {
                return;
            }
            
            foreach (var dropMino in _dropMinos)
            {
                _minoService.DestroyMino(dropMino);
            }
        }

        public void RegisterDropMino(Polyomino mino)
        {
            _dropMinos.Add(mino);
        }
        
        
        private List<Polyomino> _dropMinos = new();
        private readonly MinoService _minoService;
    }
}