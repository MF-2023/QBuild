using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UniRx;
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

        public void JointMino(Polyomino otherMino)
        {
            foreach (var block in otherMino.GetBlocks())
            {
                if (IsFalling) continue;
                block.OnBlockPlaced(_blocks[0].GetStability());
                block.SetMinoKey(_selfKey);
                _blocks.Add(block);
            }

            foreach (var rendererMaterial in from block in _blocks from renderer in block.GetComponentsInChildren<Renderer>() from rendererMaterial in renderer.materials select rendererMaterial)
            {
                rendererMaterial.color = Color.gray;
            }
            IsFalling = false;
        }

        public IEnumerator ContactMino(CancellationToken cancellationToken)
        {
            yield return Observable.Timer(System.TimeSpan.FromSeconds(1))
                .FirstOrDefault()
                .ToYieldInstruction(cancellationToken);
        }
        
        public void StartTranslate()
        {
            isTranslate = true;
        }
        
        public void FinishTranslate()
        {
            isTranslate = false;
        }
        
        public bool IsBusy()
        {
            return isTranslate;
        }
        public IReadOnlyList<Block> GetBlocks()
        {
            return _blocks;
        }

        [SerializeField] private List<Block> _blocks = new List<Block>();

        public bool IsFalling { get; private set; } = true;

        public bool isTranslate = false;
        
        [SerializeField] private MinoKey _selfKey;
    }
}