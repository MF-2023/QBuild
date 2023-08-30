using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QBuild.Mino
{
    /// <summary>
    /// ミノを構成するブロックの集合体
    /// </summary>
    [Serializable]
    public class Polyomino
    {
        public void SetDictionaryKey(MinoKey key)
        {
            _selfKey = key;
        }


        public MinoKey GetStoreKey()
        {
            return _selfKey;
        }

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }

        public void OnFall()
        {
            IsFalling = true;
            foreach (var block in _blocks)
            {
                block.OnFall();
            }
        }
        
        public void Place()
        {
            float stability = -1;
            foreach (var block in _blocks)
            {
                block.OnBlockPlaced(stability);
                stability = block.GetStability();
            }

            IsFalling = false;
        }

        public void ContactMino(Polyomino otherMino)
        {
            var material = _blocks[0].GetComponentInChildren<Renderer>().material;
            foreach (var block in otherMino.GetBlocks())
            {
                block.GetComponentsInChildren<Renderer>().ToList().ForEach(x => x.material = material);
                if (IsFalling) continue;
                block.OnBlockPlaced(_blocks[0].GetStability());
                block.SetMinoKey(_selfKey);
                _blocks.Add(block);
            }

            IsFalling = false;
        }

        public IReadOnlyList<Block> GetBlocks()
        {
            return _blocks;
        }

        [SerializeField] private List<Block> _blocks = new List<Block>();

        public bool IsFalling { get; private set; } = true;

        [SerializeField] private MinoKey _selfKey;
    }
}