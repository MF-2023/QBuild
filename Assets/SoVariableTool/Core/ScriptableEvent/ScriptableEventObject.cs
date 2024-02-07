using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace SoVariableTool
{
    [Serializable]
    public abstract class ScriptableEventObjectBase : ScriptableObject,ISerializationCallbackReceiver
    {
        public virtual Type GenericType { get; }
        public abstract UnityEventBase CreateUnityEvent();

        public void RegisterListener(EventListener eventListener)
        {
            Debug.Log($"{this.GetInstanceID()} ScriptableEventObjectBase EventListener: {eventListener.name} BindRegistration: {name}");
            EventListeners.Add(eventListener);
            Debug.Log($"EventListeners Added: {EventListeners.Count}");
        }

        public void UnregisterListener(EventListener listener)
        {
            EventListeners.Remove(listener);
            Debug.Log($"EventListeners Removed: {EventListeners.Count}");
        }

        public void Raise()
        {
            Debug.Log($"{this.GetInstanceID()} Event: {name} Raised");
            
            if (!IsPlay())
                return;
            
            Debug.Log($"EventListeners: {EventListeners.Count}");
            foreach (var eventListener in EventListeners)
            {
                Debug.Log($"OnEventRaised: {eventListener.name} Raised");
                eventListener.OnEventRaised(this, Array.Empty<object>(), _debugLogEnabled);
            }
        }

        public void Raise(object[] args)
        {
            Debug.Log($"{this.GetInstanceID()} Event: {name} Raised");

            if (!IsPlay())
                return;

            foreach (var eventListener in EventListeners)
            {
                eventListener.OnEventRaised(this, args, _debugLogEnabled);
            }
        }

        private void OnEnable()
        {
            Debug.Log($"{this.GetInstanceID()} Event: {name} Enabled");
        }

        protected bool IsPlay()
        {
#if UNITY_EDITOR
            return Application.isPlaying;
#else
            return true;
#endif
        }
        
        [SerializeField] protected bool _debugLogEnabled = false;
        protected readonly HashSet<EventListener> EventListeners = new();
        protected readonly HashSet<Object> ListenersObjects = new();
        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
        }
    }

    [Serializable]
    public abstract class ScriptableEventObject<T, TU> : ScriptableEventObjectBase
        where TU : UnityEvent<T>, IDynamicEventUseable, new()
    {
        public override Type GenericType => typeof(T);
        public override UnityEventBase CreateUnityEvent() => new TU();

        private Action<T> _onRaised = null;

        public event Action<T> OnRaised
        {
            add
            {
                _onRaised += value;

                var listener = value.Target as Object;
                if (listener != null)
                    ListenersObjects.Add(listener);
            }
            remove
            {
                _onRaised -= value;

                var listener = value.Target as Object;
                ListenersObjects.Remove(listener);
            }
        }

        public void Raise(T param)
        {
            if (!IsPlay())
                return;

            Raise(new[] {(object) param});
            _onRaised?.Invoke(param);

#if UNITY_EDITOR
            if (_debugLogEnabled)
                Debug();
#endif
        }

        public List<Object> GetAllObjects()
        {
            var allObjects = new List<Object>(EventListeners);
            allObjects.AddRange(ListenersObjects);
            return allObjects;
        }

        private void Debug()
        {
            if (_onRaised == null)
                return;
            var delegates = _onRaised.GetInvocationList();
            foreach (var del in delegates)
            {
                var sb = new StringBuilder();
                sb.Append("<color=#52D5F2>[Event] </color>");
                sb.Append(name);
                sb.Append(" => ");
                sb.Append(del.GetMethodInfo().Name);
                sb.Append("()");
                var monoBehaviour = del.Target as MonoBehaviour;
                UnityEngine.Debug.Log(sb.ToString(), monoBehaviour?.gameObject);
            }
        }
        
        [SerializeField] protected T _debugValue = default;
    }
}