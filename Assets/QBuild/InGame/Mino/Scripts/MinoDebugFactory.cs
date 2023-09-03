using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace QBuild.Mino
{
    public class MinoDebugFactory : IMinoFactory
    {

        public event Action<Polyomino> OnMinoCreated;
        
        [Inject]
        public MinoDebugFactory(BlockFactory blockFactory, MinoStore minoStore)
        {
            _blockFactory = blockFactory;
            _minoStore = minoStore;
        }
        
        public Polyomino CreateMino(MinoType minoType, Vector3Int origin, Transform parent)
        {
            var mino = new Polyomino();

            
            var key = new MinoKey(_minoStore.CreateKey());
            mino.SetDictionaryKey(key);

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
                    foreach (var rendererMaterial in renderer.materials)
                    {
                        rendererMaterial.color = color;
                    }
                }

                mino.AddBlock(block);
            }

            _minoStore.AddMino(key, mino);
            
            Debug.Log("Created Mino");
            
            OnMinoCreated?.Invoke(mino);
            return mino;
        }

        private static readonly List<Color> ColorTable = new()
        {
            Color.black,
            Color.blue,
            Color.cyan,
            Color.green,
            Color.magenta,
            Color.red,
            Color.yellow,
        };

        private readonly BlockFactory _blockFactory;
        private readonly MinoStore _minoStore;
    }
}