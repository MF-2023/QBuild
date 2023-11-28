using System;
using System.Collections.Generic;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SoVariableTool.ScriptableVariable
{
    [Serializable]
    public abstract class ScriptableVariableObjectBase : ScriptableObject
    {
        protected UnityAction<object> _onValueChanged;


        public event UnityAction<object> OnValueChanged
        {
            add
            {
                _onValueChanged += value;

                var listener = value.Target as UnityEngine.Object;
                if (listener != null && !ListenersObjects.Contains(listener))
                    ListenersObjects.Add(listener);
            }
            remove
            {
                _onValueChanged -= value;

                var listener = value.Target as UnityEngine.Object;
                if (ListenersObjects.Contains(listener))
                    ListenersObjects.Remove(listener);
            }
        }

        protected readonly List<UnityEngine.Object> ListenersObjects = new();

        public string Guid
        {
            get => _guid;
            set => _guid = value;
        }

        public virtual Type GetGenericType => null;

        public Action RepaintRequest = delegate { };

        public virtual void Reset()
        {
        }

        [SerializeField, HideInInspector] private string _guid;

        public T GetValue<T>()
        {
            var value = GetValue();
            if (value is T t)
                return t;
            return default;
        }

        public abstract object GetValue();
    }

    public enum ResetType
    {
        SceneLoaded,
        ApplicationStarts,
    }

    [Serializable]
    public abstract class ScriptableVariable<T> : ScriptableVariableObjectBase, ISerializationCallbackReceiver
    {
        [SerializeField] private T _value;
        private T _initialValue;

        [SerializeField] private bool _debugLogEnabled;

        [SerializeField] private ResetType _resetOn = ResetType.SceneLoaded;

        [SerializeField] private bool _saved;

        public T PreviousValue { get; private set; }

        public virtual T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                    return;
                _value = value;
                ValueChanged();
            }
        }

        public override object GetValue()
        {
            return Value;
        }

        private void ValueChanged()
        {
            _onValueChanged?.Invoke(_value);

            if (_saved)
                Save();

            if (_debugLogEnabled)
            {
                var log = GetColorizedString();
                log += _saved ? LogUtilities.GenerateTag(Color.green, "Saved") : "";
                Log(log);
            }

            PreviousValue = _value;
#if UNITY_EDITOR
            SetDirtyAndRepaint();
#endif
        }

        public override Type GetGenericType => typeof(T);

        private void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#else
            Init();
#endif
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_resetOn != ResetType.SceneLoaded)
                return;

            if (mode != LoadSceneMode.Single) return;

            if (_saved)
                Load();
            else
                ResetToInitialValue();
        }

#if UNITY_EDITOR
        private void SetDirtyAndRepaint()
        {
            EditorUtility.SetDirty(this);
            RepaintRequest?.Invoke();
        }

        public void OnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
        {
            if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
                Init();
            else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
                if (!_saved)
                    ResetToInitialValue();
        }

        protected virtual void OnValidate()
        {
            if (Equals(_value, PreviousValue))
                return;
            ValueChanged();
        }

        public override void Reset()
        {
            Value = default;
            _initialValue = default;
            PreviousValue = default;
            _saved = false;
            _resetOn = ResetType.SceneLoaded;
            _debugLogEnabled = false;
            ListenersObjects.Clear();
        }
#endif
        public void Init()
        {
            _initialValue = _value;
            PreviousValue = _value;
            if (_saved)
                Load();
            ListenersObjects.Clear();
        }

        public void ResetToInitialValue()
        {
            Value = _initialValue;
            PreviousValue = _initialValue;
        }

        public virtual void Save()
        {
            VariableUtility.Save(this);
        }

        public virtual void Load()
        {
            Value = VariableUtility.Load(this);

            PreviousValue = _value;

            if (_debugLogEnabled)
                Log(GetColorizedString() + LogUtilities.GenerateTag(Color.green, "Loaded"));
        }

        public override string ToString()
        {
            var sb = new StringBuilder(name);
            sb.Append(" : ");
            sb.Append(_value);
            sb.Append(" ");
            sb.Append($"[{Guid}] ");
            return sb.ToString();
        }

        private void Log(string message)
        {
            Debug.Log(message, this);
        }

        private string GetColorizedString() => LogUtilities.ColorTagMessage(Color.cyan, "Variable", ToString());

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            if (_resetOn == ResetType.ApplicationStarts)
            {
                if (_saved)
                    Load();
                else
                    ResetToInitialValue();
            }
        }
    }
}