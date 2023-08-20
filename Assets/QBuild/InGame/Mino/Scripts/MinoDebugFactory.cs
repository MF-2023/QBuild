using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace QBuild.Mino
{
    public class MinoDebugFactory : IMinoFactory
    {
        [Inject]
        public MinoDebugFactory(BlockFactory blockFactory, MinoStore minoStore)
        {
            _blockFactory = blockFactory;
            _minoStore = minoStore;
        }


        public Polyomino CreateMino(MinoType minoType, Vector3Int origin, Transform parent)
        {
            var polyomino = new Polyomino();

            var key = new MinoKey(_minoStore.Count);
            polyomino.SetDictionaryKey(key);

            var color = ColorTable[_minoStore.Count % ColorTable.Count];
            //カラーテーブルを周回するごとに色を明るくする
            var t = _minoStore.Count / ColorTable.Count / 5f;
            color = Color.Lerp(color, Color.white, t);


            foreach (var positionToBlockType in minoType.GetBlockTypes())
            {
                var position = positionToBlockType._pos + origin;
                var block = _blockFactory.CreateBlock(positionToBlockType._blockType, position, parent);
                block.SetMinoKey(key);
                block.name = $"Block {position}";

                foreach (var renderer in block.GetComponentsInChildren<Renderer>())
                {
                    renderer.material.color = color;
                }

                polyomino.AddBlock(block);
            }

            _minoStore.AddMino(key, polyomino);
            return polyomino;
        }

        private static readonly List<Color> ColorTable = new()
        {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.gray,
            Color.green,
            Color.magenta,
            Color.red,
            Color.yellow,
        };

        private readonly BlockFactory _blockFactory;
        private readonly MinoStore _minoStore;
    }
}