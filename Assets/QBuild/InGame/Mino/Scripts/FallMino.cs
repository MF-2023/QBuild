using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Mino
{
    public class FallMino
    {
        /// <summary>
        /// 落下開始時イベント
        /// </summary>
        public event Action<Polyomino> OnMinoFall;
        
        /// <summary>
        /// 落下更新時イベント
        /// </summary>
        public event Action<Polyomino,int> OnMinoDown;

        [Inject]
        public FallMino()
        {
        }

        public void Fall(Polyomino mino)
        {
            Debug.Log($"Fall Mino:{mino.GetStoreKey()}");
            mino.OnFall();
            _fallingMino = mino;
            OnMinoFall?.Invoke(mino);
        }

        public void Down(int moveY)
        {
            if (_fallingMino == null) return;
            Debug.Log("Mino Down");
            OnMinoDown?.Invoke(_fallingMino, moveY);
        }

        private Polyomino _fallingMino;
    }
}