using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace QBuild.Mino
{
    /// <summary>
    /// Polyomino の生成を行う
    /// </summary>
    public class MinoFactory : IMinoFactory
    {
        public event Action<Polyomino> OnMinoCreated;

        [Inject]
        public MinoFactory(BlockFactory blockFactory, MinoStore minoStore)
        {
            _blockFactory = blockFactory;
            _minoStore = minoStore;
        }


        public Polyomino CreateMino(MinoType minoType, Vector3Int origin, Transform parent)
        {
            var mino = new Polyomino();

            var key = new MinoKey(_minoStore.Count);
            mino.SetDictionaryKey(key);

            foreach (var positionToBlockType in minoType.GetBlockTypes())
            {
                var position = positionToBlockType._pos + origin;
                var block = _blockFactory.CreateBlock(positionToBlockType._blockType, position, parent);
                block.SetMinoKey(key);
                block.name = $"Block {position}";

                mino.AddBlock(block);
            }

            _minoStore.AddMino(key, mino);
            OnMinoCreated?.Invoke(mino);
            return mino;
        }

        private readonly BlockFactory _blockFactory;
        private readonly MinoStore _minoStore;
    }
}