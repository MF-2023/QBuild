using System;
using System.Collections.Concurrent;
using UnityEngine;


namespace SoVariableTool.Binding.Converter
{
    public static class ConverterRepository
    {
        private static readonly ConcurrentDictionary<Tuple<Type, Type>, Func<object, object>>
            Converters = new();

        private static bool _initialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            Converters.Clear();
            _initialized = false;
            AddConverter((int num) => num.ToString());
            AddConverter((long num) => num.ToString());
            AddConverter((float num) => num.ToString());
            AddConverter((double num) => num.ToString());

            _initialized = true;
        }

        public static Func<object, object> GetConverter<TFrom, TTo>()
        {
            return GetConverter(typeof(TFrom), typeof(TTo));
        }

        public static Func<object, object> GetConverter(Type sourceType, Type targetType)
        {
            if (!_initialized)
            {
                Initialize();
            }

            var key = new Tuple<Type, Type>(sourceType, targetType);
            if (Converters.TryGetValue(key, out var converter))
            {
                return converter;
            }

            return null;
        }

        public static void AddConverter(ITypeConvertable converter)
        {
            var key = new Tuple<Type, Type>(converter.FromType, converter.ToType);
            if (Converters.ContainsKey(key))
            {
                return;
            }

            Converters[key] = converter.Convert;
        }

        public static void AddConverter<TFrom, TTo>(Func<TFrom, TTo> converter)
        {
            var key = new Tuple<Type, Type>(typeof(TFrom), typeof(TTo));
            if (Converters.ContainsKey(key))
            {
                return;
            }

            Converters[key] = o => converter((TFrom)o);
        }

        public delegate bool TryConvertDelegate<in TFrom, TTo>(TFrom a, Type toType, out TTo b);

        public static void AddConverter<TFrom, TTo>(TryConvertDelegate<object, TTo> tryConvert)
        {
            var key = new Tuple<Type, Type>(typeof(TFrom), typeof(TTo));
            if (Converters.ContainsKey(key))
            {
                return;
            }

            if (tryConvert == null) return;
            Converters[key] = o => tryConvert(o, typeof(TTo), out var result) ? result : default;
            return;
        }
    }
}