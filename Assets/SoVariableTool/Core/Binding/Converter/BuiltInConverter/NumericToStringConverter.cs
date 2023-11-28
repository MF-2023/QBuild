using System;

namespace SoVariableTool.Binding.Converter
{
    public class IntToStringConverter : ITypeConvertable
    {
        public Type FromType => typeof(int);
        public Type ToType => typeof(string);
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return fromType == FromType && toType == ToType;
        }

        public object Convert(object source)
        {
            return source.ToString();
        }
    }
    
    public class LongToStringConverter : ITypeConvertable
    {
        public Type FromType => typeof(long);
        public Type ToType => typeof(string);
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return fromType == FromType && toType == ToType;
        }

        public object Convert(object source)
        {
            return source.ToString();
        }
    }
    
    public class FloatToStringConverter : ITypeConvertable
    {
        public Type FromType => typeof(float);
        public Type ToType => typeof(string);
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return fromType == FromType && toType == ToType;
        }

        public object Convert(object source)
        {
            return source.ToString();
        }
    }
    
    public class DoubleToStringConverter : ITypeConvertable
    {
        public Type FromType => typeof(double);
        public Type ToType => typeof(string);
        
        public bool CanConvert(Type fromType, Type toType)
        {
            return fromType == FromType && toType == ToType;
        }

        public object Convert(object source)
        {
            return source.ToString();
        }
    }
}