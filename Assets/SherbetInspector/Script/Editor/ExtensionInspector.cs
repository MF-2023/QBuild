using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SherbetInspector.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace SherbetInspector.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object),true)]
    public class ExtensionInspector : UnityEditor.Editor
    {
        private List<SerializedProperty> _serializedProperties = new List<SerializedProperty>();
        private IEnumerable<FieldInfo> _nonSerializedFields;
        private IEnumerable<PropertyInfo> _nativeProperties;
        private IEnumerable<MethodInfo> _methods;
        private Dictionary<string, SavedBool> _foldouts = new Dictionary<string, SavedBool>();

        protected void OnEnable()
        {
            _methods = ReflectionUtility.GetAllMethods(
                target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
        }

        public override void OnInspectorGUI()
        {
            
            DrawDefaultInspector();
            
            DrawButtons();
        }

        protected void DrawButtons()
        {
            if (!_methods.Any()) return;
            
            foreach (var method in _methods)
            {
                SherbetEditorGUI.Button(serializedObject.targetObject,method);
            }
        }
    }
}