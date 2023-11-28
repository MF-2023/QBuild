using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SoVariableTool.Binding.Transformer
{
    public abstract class BindingValueTransformer : ScriptableObject
    {
        [SerializeField] private bool _enabled = true;
        public bool Enabled => _enabled;

        protected abstract HashSet<Type> FromTypes { get; }

        protected abstract HashSet<Type> ToTypes { get; }

        public IEnumerable<Type> GetFromTypes => FromTypes;

        public IEnumerable<Type> GetToTypes => ToTypes;

        public abstract object Transform(object source, object target);

        public virtual bool CanFormat(Type fromType, Type toType)
        {
            return CanFormatFrom(fromType) && CanFormatTo(toType);
        }

        public virtual bool CanFormatFrom(Type fromType)
        {
            return FromTypes.Contains(fromType);
        }

        public virtual bool CanFormatTo(Type toType)
        {
            return ToTypes.Contains(toType);
        }

        public bool CanFormat<TFrom, TTo>() => CanFormat(typeof(TFrom), typeof(TTo));

        public bool CanFormatFrom<TFrom>() => CanFormatFrom(typeof(TFrom));

        public bool CanFormatTo<TTo>() => CanFormatTo(typeof(TTo));
    }
}