using System;
using UnityEngine;

namespace QBuild.Gimmick.TriggerPattern
{
    public class CollisionTrigger : ITrigger
    {
        public event Action OnActive;
        public event Action OnDisable;
        public void TriggerBind(GimmickTrigger gimmick)
        {
            gimmick.OnEnter += OnEnter;
            gimmick.OnExit += OnExit;
        }
        
        private void OnEnter(Collider other)
        {
            OnActive?.Invoke();
        }
        
        private void OnExit(Collider other)
        {
            OnDisable?.Invoke();
        }
    }
}