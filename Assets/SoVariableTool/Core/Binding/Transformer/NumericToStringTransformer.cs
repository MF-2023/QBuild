using System;
using System.Collections.Generic;

namespace SoVariableTool.Binding.Transformer
{
    public class NumericToStringTransformer : BindingValueTransformer
    {
        protected override HashSet<Type> FromTypes => new() { typeof(int), typeof(long) , typeof(float) };
        protected override HashSet<Type> ToTypes => new() { typeof(string) };

        public override object Transform(object source, object target)
        {
            if (source == null) return null;
            if (!Enabled) return source;
            
            return source.ToString();
        }
    }
}