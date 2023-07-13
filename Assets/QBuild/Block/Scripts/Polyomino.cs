using System;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild
{
    
    [Serializable]
    public class Polyomino
    {
        [SerializeField] private List<Block> _blocks = new List<Block>();

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
        }
    }
}