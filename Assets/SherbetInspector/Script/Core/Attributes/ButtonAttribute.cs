using System;
using UnityEngine;

namespace SherbetInspector.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : SherbetInspectorAttribute
    {
        public string DisplayName { get; private set; }
        
        public ButtonAttribute(string displayName = null)
        {
            DisplayName = displayName;
        }
    }
}