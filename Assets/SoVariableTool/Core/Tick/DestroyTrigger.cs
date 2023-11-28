using System;
using UnityEngine;

namespace SoVariableTool.Tick
{
    public class DestroyTrigger : MonoBehaviour
    {
        private bool _callDestroy = false;
        private Action OnDestroyAction;
        public bool IsActivated { get; private set; }
        
        void Awake()
        {
            IsActivated = true;
        }
    }
}