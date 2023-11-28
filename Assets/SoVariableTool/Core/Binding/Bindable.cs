using System;
using SoVariableTool.Binding.Converter;
using SoVariableTool.Tick;
using UnityEngine;

namespace SoVariableTool.Binding
{
    public enum ConnectionType
    {
        Sender,
        Receiver
    }

    public enum OnBindBehaviour
    {
        None,
        SetValue,
        GetValue
    }

    [Serializable]
    public class Bindable : ITickable
    {
        /// <summary>
        /// Binderで登録される際に、BinderのGameObjectを設定する
        /// </summary>
        internal GameObject Owner { get; set; }

        [SerializeField] private BindingValue _bindingValue;
        [SerializeField] private Ticker _ticker;
        [SerializeField] private ConnectionType _connectionType;
        public ConnectionType ConnectionType => _connectionType;
        public Type ValueType => _bindingValue?.ValueType;

        [SerializeField] private OnBindBehaviour _onBindBehaviour;

        public Guid Guid { get; }
        public Bind Bind { get; internal set; }

        public Action OnValueChanged = delegate { };

        public Bindable()
        {
            Guid = Guid.NewGuid();
            _ticker = new Ticker(this);
        }

        public void Initialize()
        {
            _bindingValue.Initialize();
            _bindingValue.OnValueChanged += NotifyChange;
            _ticker.Initialize(Owner);
        }

        public object Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        public object GetValue()
        {
            return _bindingValue.GetValue();
        }

        public void SetValue(object value)
        {
            _bindingValue.SetValue(value);
            NotifyChange();
        }

        public void SetValueWithoutNotify(object newValue)
        {
            _bindingValue.SetValue(newValue);
        }

        public void RunOnBind()
        {
            switch (_onBindBehaviour)
            {
                case OnBindBehaviour.None:
                    break;
                case OnBindBehaviour.SetValue:
                    Bind.NotifyChange(Guid);
                    break;
                case OnBindBehaviour.GetValue:
                    // 1フレーム後に実行する
                    // 他のBindableの初期化が終わっていない可能性がある為
                    _ticker.ExecuteAtEndOfFrame(() =>
                        ProcessValue(Bind, Bind.LastBindable, this)
                    );
                    break;
            }
        }

        private void NotifyChange()
        {
            if (_connectionType == ConnectionType.Receiver) return;
            Bind.NotifyChange(Guid);
            OnValueChanged?.Invoke();
        }

        public void Tick()
        {
            _bindingValue?.HasValueChanged();
        }

        public void StartTick()
        {
            if (_connectionType == ConnectionType.Receiver)
                return;

            _ticker.StartTicking();
        }

        public void StopTick()
        {
            _ticker.StopTicking();
        }

        public static void ProcessValue(Bind bind, Bindable sourceBindable, Bindable targetBindable)
        {
            var sourceValue = sourceBindable.Value;
            var sourceValueType = sourceBindable.ValueType;

            var targetValue = targetBindable.Value;
            var targetValueType = targetBindable.ValueType;

            if (sourceValueType != targetValueType)
            {
                var converter = ConverterRepository.GetConverter(sourceValueType, targetValueType);
                if (converter == null)
                {
                    //TODO: エラー処理
                    return;
                }

                targetBindable.SetValueWithoutNotify(converter(sourceValue));
                return;
            }

            targetBindable.SetValueWithoutNotify(sourceValue);
        }
    }
}