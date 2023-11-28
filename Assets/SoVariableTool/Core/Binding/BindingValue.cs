using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SoVariableTool.Binding
{
    public enum MemberVariableType
    {
        None,
        Field,
        Property,
    }


    /// <summary>
    /// ターゲットオブジェクトと変数名から値にバインドするためのクラス。
    /// </summary>
    [Serializable]
    public class BindingValue
    {
        [SerializeField] private Object _targetObject = null;
        public Object TargetObject => _targetObject;

        [SerializeField] private string _variableName = "";
        public string VariableName => _variableName;

        [SerializeField] private MemberVariableType _memberVariableType = MemberVariableType.None;
        public MemberVariableType MemberVariableType => _memberVariableType;

        private object _lastValue = null;

        private class PropertyGetSet
        {
            public FieldInfo TargetField { get; private set; }
            public PropertyInfo TargetProperty { get; private set; }
            private Func<object> _getter = null;
            public Action<object> _setter = null;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public object GetValue()
            {
                if (_getter == null) return default;
                return _getter.Invoke();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void SetValue(object val)
            {
                if (_setter == null) return;
                _setter.Invoke(val);
            }

            public bool InitializeMember(MemberVariableType memberVariableType, UnityEngine.Object target,
                string variableName)
            {
                if (target == null) return false;
                switch (memberVariableType)
                {
                    case MemberVariableType.None:
                        return false;
                        break;
                    case MemberVariableType.Field:
                        var field = target.GetType().GetField(variableName);
                        Debug.Log("GetField" + variableName + $":{target.GetType().FullName}");
                        if (field == null) return false;
                        TargetField = field;
                        Debug.Log("Finish");
                        break;
                    case MemberVariableType.Property:
                        var property = target.GetType().GetProperty(variableName);
                        if (property == null) return false;
                        TargetProperty = property;
                        break;
                }

                return true;
            }

            public void InitializeGetSet(MemberVariableType memberVariableType, UnityEngine.Object target)
            {
                if (memberVariableType == MemberVariableType.Property)
                {
                    if (TargetProperty == null) return;
                    if (TargetProperty.CanRead)
                    {
                        _getter = () => TargetProperty.GetValue(target, null);
                    }

                    if (TargetProperty.CanWrite) _setter = val => TargetProperty.SetValue(target, val, null);
                }
                else if (memberVariableType == MemberVariableType.Field)
                {
                    if (TargetField == null) return;
                    _getter = () => TargetField.GetValue(target);
                    _setter = val => TargetField.SetValue(target, val);
                }
            }

            public bool IsValid()
            {
                return _getter != null && _setter != null;
            }
        }

        private PropertyGetSet _propertyProxy = new();

        private bool _isInitialized = false;

        public Type ValueType
        {
            get
            {
                if (!_isInitialized) return null;
                if (!_propertyProxy.IsValid()) return null;

                return _memberVariableType switch
                {
                    MemberVariableType.None => null,
                    MemberVariableType.Field => _propertyProxy.TargetField != null
                        ? _propertyProxy.TargetField.FieldType
                        : null,
                    MemberVariableType.Property => _propertyProxy.TargetProperty != null
                        ? _propertyProxy.TargetProperty.PropertyType
                        : null,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public event Action OnValueChanged = () => { };

        public object GetValue()
        {
            if (!_isInitialized) Initialize();
            if (!_isInitialized) return default;


            return _propertyProxy.GetValue();
        }

        public void SetValue(object newValue)
        {
            if (!_isInitialized) Initialize();
            if (!_isInitialized) return;

            if (Equals(newValue, GetValue())) return;
            _propertyProxy.SetValue(newValue);
            OnValueChanged.Invoke();
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            _propertyProxy = new PropertyGetSet();
            if (!InitProperty()) return;
            _propertyProxy.InitializeGetSet(_memberVariableType, _targetObject);
            _isInitialized = _propertyProxy.IsValid();

            _lastValue = GetValue();
        }

        public void HasValueChanged()
        {
            if (!_isInitialized) Initialize();
            if (!_isInitialized) return;

            var newValue = GetValue();
            if (Equals(newValue, _lastValue)) return;
            _lastValue = newValue;
            OnValueChanged.Invoke();
        }

        public void SetBind(Object target, string variableName, MemberVariableType memberVariableType)
        {
            _targetObject = target;
            _variableName = variableName;
            _memberVariableType = memberVariableType;
            _propertyProxy = new PropertyGetSet();
            _propertyProxy.InitializeMember(_memberVariableType, _targetObject, _variableName);
            _propertyProxy.InitializeGetSet(_memberVariableType, _targetObject);
            _isInitialized = _propertyProxy.IsValid();
            _lastValue = GetValue();
            
            Debug.Log($"type:{_memberVariableType} name:{_variableName} setter:{_propertyProxy._setter != null}");
        }

        private bool InitProperty()
        {
            if (_targetObject == null) return false;

            return _propertyProxy.InitializeMember(_memberVariableType, _targetObject, _variableName);
        }
    }
}