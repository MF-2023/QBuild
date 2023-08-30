using System;
using VContainer;
using VContainer.Unity;

namespace QBuild.Mino.ProvisionalMino
{
    /// <summary>
    /// フローの制御を行う
    /// </summary>
    public class ProvisionalMinoPresenter : IStartable, IDisposable
    {
        [Inject]
        public ProvisionalMinoPresenter(MinoService minoService, ProvisionalMinoView provisionalMinoView,
            ProvisionalMinoService provisionalMinoService)
        {
            _minoService = minoService;
            _provisionalMinoView = provisionalMinoView;
            _provisionalMinoService = provisionalMinoService;
        }

        public void Start()
        {
            _minoService.OnMinoMoved += OnMoved;
        }

        public void Dispose()
        {
            _minoService.OnMinoMoved -= OnMoved;
        }

        private void OnMoved(Polyomino mino)
        {
            var positions = _provisionalMinoService.GetProvisionalPlacePosition(mino);
            _provisionalMinoView.SetPosition(positions);
        }


        private readonly MinoService _minoService;
        private readonly ProvisionalMinoView _provisionalMinoView;
        private readonly ProvisionalMinoService _provisionalMinoService;
    }
}