using System;
using UnityEngine;

namespace SherbetInspector.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false)]
    public class TypeLabelAttribute : Attribute
    {
        public string Name { get; private set; }

        public TypeLabelAttribute(string name)
        {
            Name = name;
        }
    }
}