using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoVariableTool
{
    public class EventListener : MonoBehaviour
    {
        [SerializeField] public EventResponse[] _eventResponses = null;

        public void OnEventRaised(ScriptableEventObjectBase scriptableEventRaised, object[] param, bool debug = false)
        {
            _dictionary[scriptableEventRaised].Invoke(param);
        }


        private void BindRegistration()
        {
            foreach (var eventResponse in _eventResponses)
            {
                eventResponse.ScriptableEvent.RegisterListener(this);
                _dictionary.TryAdd(eventResponse.ScriptableEvent, eventResponse.Response);
            }
        }
        private void UnbindRegistration()
        {
            foreach (var eventResponse in _eventResponses)
            {
                eventResponse.ScriptableEvent.UnregisterListener(this);
                _dictionary.Remove(eventResponse.ScriptableEvent);
            }
        }

        private void OnEnable()
        {
            BindRegistration();
        }

        private void OnDisable()
        {
            UnbindRegistration();
        }

        private readonly Dictionary<ScriptableEventObjectBase, UnityEventDynamic> _dictionary = new();
    }


    [Serializable]
    public class EventResponse
    {
        [SerializeReference] private ScriptableEventObjectBase _scriptableEvent = null;
        public ScriptableEventObjectBase ScriptableEvent => _scriptableEvent;

        [SerializeField] private UnityEventDynamic _response = new();
        public UnityEventDynamic Response => _response;
    }
}