using System;
using System.Collections;
using UnityEngine;

namespace SoVariableTool.Tick
{
    [Serializable]
    public class Ticker : IDisposable
    {
        public ITickable Tickable { get; }
        public bool IsRunning { get; private set; }

        private Coroutine TickerCoroutine { get; set; }

        private TickCoroutine _tickCoroutine;

        public Ticker(ITickable tickable)
        {
            Tickable = tickable;
        }

        public void Initialize(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out _tickCoroutine))
            {
                _tickCoroutine = gameObject.AddComponent<TickCoroutine>();
            }
        }
        public void Tick()
        {
            Tickable.Tick();
        }

        public void StartTicking()
        {
            if (IsRunning) StopTicking();
            IsRunning = true;
            _tickCoroutine.StartCoroutine(GetIEnumerator());
        }

        public void StopTicking()
        {
            IsRunning = false;
            if (TickerCoroutine == null) return;
            _tickCoroutine.StopCoroutine(TickerCoroutine);
            TickerCoroutine = null;
        }
        
        private IEnumerator GetIEnumerator()
        {
            int frameCount = 0;
            while (IsRunning)
            {
                yield return null;
                frameCount++;
                if (frameCount < 1) continue;
                frameCount = 0;
                Tick();
            }

            if (TickerCoroutine != null)
                _tickCoroutine.StopCoroutine(TickerCoroutine);

            TickerCoroutine = null;
        }

        public void Dispose()
        {
            if (_tickCoroutine != null) StopTicking();
        }
        
        public void ExecuteAtEndOfFrame(Action callback)
        {
            if (_tickCoroutine == null) return;
            _tickCoroutine.StartCoroutine(DelayExecutionToTheEndOfFrame(callback));
        }
        private static IEnumerator DelayExecutionToTheEndOfFrame(Action callback)
        {
            yield return new WaitForEndOfFrame();
            callback?.Invoke();
        }
    }
}