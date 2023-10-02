using System;
using Cysharp.Threading.Tasks;
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
        public event Action<Polyomino, int, UniTaskCompletionSource> OnMinoDown;

        public event Action<Polyomino> OnMinoDone;
        
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

        public async UniTask Down(int moveY)
        {
            if (_fallingMino == null) return;

            var source = new UniTaskCompletionSource();
            OnMinoDown?.Invoke(_fallingMino, moveY, source);

            await source.Task;
        }

        public bool IsBusy()
        {
            if (_fallingMino == null) return false;
            return _fallingMino.IsBusy();
        }

        public void MinoDone()
        {
            if(_fallingMino == null) return;
            OnMinoDone?.Invoke(_fallingMino);
        }

        private Polyomino _fallingMino;
    }
}