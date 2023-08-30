using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace QBuild.Mino
{
    public class MinoPhysicsSimulation
    {
        public event Action<List<Polyomino>> OnDropBlocks;

        [Inject]
        public MinoPhysicsSimulation(StabilityCalculator stabilityCalculator, BlockService blockService,MinoService minoService)
        {
            _stabilityCalculator = stabilityCalculator;
            _blockService = blockService;
            _minoService = minoService;
        }

        public void Execute(Polyomino mino)
        {
            var dropMinos = new HashSet<MinoKey>();
            foreach (var block in mino.GetBlocks())
            {
                var list = _stabilityCalculator.CalcPhysicsStabilityToFall(block.GetGridPosition(), 32,
                    out var stability);

                if (!list.Any()) continue;
                Debug.Log($"list:{list.Count} stability:{stability}");
                foreach (var pos in list)
                {
                    Debug.Log($"pos:{pos}");
                    _blockService.TryGetBlock(pos, out var dropBlock);
                    dropMinos.Add(dropBlock.GetMinoKey());
                }
            }

            var result = dropMinos.Select(key =>
            {
                _minoService.TryGetMino(key, out var dropMino);
                return dropMino;
            }).ToList();
            
            OnDropBlocks?.Invoke(result);
        }

        private readonly StabilityCalculator _stabilityCalculator;
        private readonly BlockService _blockService;
        private readonly MinoService _minoService;
    }
}