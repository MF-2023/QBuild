﻿using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace QBuild.Mino
{
    public class MinoFactory : IMinoFactory
    {
        [Inject]
        public MinoFactory(BlockManager blockManager, BlockFactory blockFactory, GameObject blockPrefab)
        {
            _blockManager = blockManager;
            _blockFactory = blockFactory;
            _blockPrefab = blockPrefab;
        }


        public Polyomino CreateMino(MinoType minoType, Vector3Int origin, Transform parent)
        {
            var polyomino = new Polyomino();

            var key = 0;
            polyomino.SetDictionaryKey(key);

            foreach (var positionToBlockType in minoType.GetBlockTypes())
            {
                var position = positionToBlockType._pos + origin;
                var block = _blockFactory.CreateBlock(positionToBlockType._blockType, position, parent);
                block.name = $"Block {position}";
                polyomino.AddBlock(block);

                //_blocks.Add(block);
            }

            return polyomino;
        }

        private readonly BlockManager _blockManager;
        private readonly BlockFactory _blockFactory;
        private readonly GameObject _blockPrefab;
    }
}