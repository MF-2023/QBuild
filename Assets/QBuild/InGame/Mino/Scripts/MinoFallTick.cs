using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Mino
{
    public class MinoFallTick
    {
        [Inject]
        public MinoFallTick(FallMino fallMino)
        {
            _fallMino = fallMino;
        }
        
        public void Tick()
        {
            
            _tick += Time.deltaTime;

            if (_tick < 0.4f) return;
            _tick = 0;

            _fallMino.Down(1);
        }

        private float _tick = 0;

        private readonly FallMino _fallMino;
    }
}