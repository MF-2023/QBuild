using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace QBuild.Mino
{
    public class DestroyMinos
    {
        public event Action<Block> OnBlockDestroy; 

        [Inject]
        public DestroyMinos(BlockService blockService)
        {
            _blockService = blockService;
        }
        
        public void AddBlock(Vector3Int block)
        {
            _removeBlocks.Add(block);
        }
        
        public void Destroy()
        {
            foreach (var blockPosition in _removeBlocks)
            {
                _blockService.TryGetBlock(blockPosition, out var destroyBlock);
                _blockService.RemoveBlock(destroyBlock);
                OnBlockDestroy?.Invoke(destroyBlock);
            }
        }
        
        private readonly HashSet<Vector3Int> _removeBlocks = new();
        private readonly BlockService _blockService;
    }
}