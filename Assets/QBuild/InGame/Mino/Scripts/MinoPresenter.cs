using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Mino
{
    public class MinoPresenter : IStartable, IDisposable
    {
        [Inject]
        public MinoPresenter(IMinoFactory minoFactory, MinoInput minoInput, MinoService minoService, FallMino fallMino,
            MinoPhysicsSimulation minoPhysicsSimulation)
        {
            Debug.Log("MinoPresenter.Inject");

            _minoFactory = minoFactory;
            _minoInput = minoInput;
            _minoService = minoService;
            _fallMino = fallMino;
            _minoPhysicsSimulation = minoPhysicsSimulation;
        }


        public void Start()
        {
            Debug.Log("MinoPresenter.Start");
            _minoFactory.OnMinoCreated += _fallMino.Fall;
            _fallMino.OnMinoFall += OnMinoFall;
            _fallMino.OnMinoDown += OnMinoDown;
            _minoService.OnMinoPlaced += OnMinoPlaced;
            
            
            _minoPhysicsSimulation.OnDropBlocks += OnMinoSimulated;
        }

        public void Dispose()
        {
            _minoFactory.OnMinoCreated -= _fallMino.Fall;
            _fallMino.OnMinoFall -= OnMinoFall;
            _fallMino.OnMinoDown -= OnMinoDown;
            _minoService.OnMinoPlaced -= OnMinoPlaced;

            _minoPhysicsSimulation.OnDropBlocks -= OnMinoSimulated;
        }

        private void OnMinoFall(Polyomino mino)
        {
            _minoMoveSubscription = _minoInput.OnMinoMove.Subscribe(x => _minoService.TranslationMino(mino, x));
        }

        private void OnMinoDown(Polyomino mino, int moveY)
        {
            _minoService.TranslationMino(mino, new Vector3Int(0, -moveY, 0));
        }

        private void OnMinoPlaced(Polyomino mino)
        {
            _minoMoveSubscription?.Dispose();
            
            _minoPhysicsSimulation.Execute(mino);
        }

        private void OnMinoSimulated(List<Polyomino> minos)
        {
            Debug.Log("OnMinoSimulated");
            foreach (var mino in minos)
            {
                _minoService.DestroyMino(mino);
            }
        }
        
        

        private readonly IMinoFactory _minoFactory;
        private readonly MinoInput _minoInput;
        private readonly FallMino _fallMino;
        private readonly MinoService _minoService;
        private readonly MinoPhysicsSimulation _minoPhysicsSimulation;

        private IDisposable _minoMoveSubscription;
    }
}