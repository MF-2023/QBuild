using System;

namespace SoVariableTool.Binding.Converter
{
    public interface ITypeConvertable
    {
        Type FromType { get; }
        Type ToType { get; }
        
        bool CanConvert(Type fromType, Type toType);
        object Convert(object source);
    }
}