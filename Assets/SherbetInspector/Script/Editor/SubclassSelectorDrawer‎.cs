using SherbetInspector.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SherbetInspector.Editor
{
    [CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        bool initialized = false;
        Type[] inheritedTypes;
        string[] typePopupNameArray;

        string[] typeFullNameArray;

        Dictionary<string, int> typeIndexDictionary = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                if (!initialized)
                {
                    Initialize(property);
                    GetCurrentTypeIndex(property.managedReferenceFullTypename);
                    initialized = true;
                }

                var currentTypeIndex = 0;

                if (!typeIndexDictionary.ContainsKey(property.propertyPath))
                {
                    typeIndexDictionary.Add(property.propertyPath,
                        Array.IndexOf(typeFullNameArray, property.managedReferenceFullTypename));
                }

                currentTypeIndex = Array.IndexOf(typeFullNameArray, property.managedReferenceFullTypename);
                var selectedTypeIndex =
                    EditorGUI.Popup(GetPopupPosition(position), currentTypeIndex, typePopupNameArray);

                UpdatePropertyToSelectedTypeIndex(property, selectedTypeIndex);

                EditorGUI.PropertyField(position, property, label, true);
            }
            else
            {
                EditorGUI.LabelField(position, label, new GUIContent("The property type is not manage reference."));
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private void Initialize(SerializedProperty property)
        {
            var utility = (SubclassSelectorAttribute) attribute;
            GetAllInheritedTypes(GetType(property), utility.IsIncludeMono());
            GetInheritedTypeNameArrays();
        }

        private void GetCurrentTypeIndex(string typeFullName)
        {
            //currentTypeIndex = Array.IndexOf(typeFullNameArray, typeFullName);
        }

        private static PropertyDrawer GetCustomPropertyDrawer(SerializedProperty property)
        {
            var propertyType = ManagedReferenceUtility.GetType(property.managedReferenceFullTypename);
            if (propertyType != null && PropertyDrawerCache.TryGetPropertyDrawer(propertyType, out var drawer))
            {
                return drawer;
            }

            return null;
        }

        void GetAllInheritedTypes(Type baseType, bool includeMono)
        {
            Type monoType = typeof(MonoBehaviour);
            inheritedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && p.IsClass && (!monoType.IsAssignableFrom(p) || includeMono))
                .Prepend(null)
                .ToArray();
        }

        private void GetInheritedTypeNameArrays()
        {
            string GetTypeName(Type type)
            {
                var label = Attribute.GetCustomAttribute(type, typeof(TypeLabelAttribute)) as TypeLabelAttribute;
                return label == null ? type.FullName : label.Name;
            }

            typePopupNameArray = inheritedTypes.Select(type => type == null ? "<null>" : $"{GetTypeName(type)}")
                .ToArray();
            typeFullNameArray = inheritedTypes.Select(type =>
                    type == null ? " " : $"{type.Assembly.ToString().Split(',')[0]} {type.FullName}")
                .ToArray();
        }

        private void UpdatePropertyToSelectedTypeIndex(SerializedProperty property, int selectedTypeIndex)
        {
            if (!typeIndexDictionary.ContainsKey(property.propertyPath))
            {
                return;
            }

            if (typeIndexDictionary[property.propertyPath] == selectedTypeIndex) return;
            typeIndexDictionary[property.propertyPath] = selectedTypeIndex;
            var selectedType = inheritedTypes[selectedTypeIndex];
            property.managedReferenceValue =
                selectedType == null ? null : Activator.CreateInstance(selectedType);

            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private static Rect GetPopupPosition(Rect currentPosition)
        {
            var popupPosition = new Rect(currentPosition);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }

        private static Type GetType(SerializedProperty property)
        {
            var typeSplit = property.managedReferenceFieldTypename.Split(' ');
            var assemblyName = typeSplit[0];
            var className = typeSplit[1];
            var assembly = Assembly.Load(assemblyName);
            var fieldType = assembly.GetType(className);
            return fieldType;
        }
    }

    public static class ManagedReferenceUtility
    {
        public static object SetManagedReference(this SerializedProperty property, Type type)
        {
            object obj = (type != null) ? Activator.CreateInstance(type) : null;
            property.managedReferenceValue = obj;
            return obj;
        }

        public static Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            int splitIndex = typeName.IndexOf(' ');
            var assembly = Assembly.Load(typeName.Substring(0, splitIndex));
            return assembly.GetType(typeName.Substring(splitIndex + 1));
        }
    }

    public static class PropertyDrawerCache
    {
        static readonly Dictionary<Type, PropertyDrawer> s_Caches = new Dictionary<Type, PropertyDrawer>();

        public static bool TryGetPropertyDrawer(Type type, out PropertyDrawer drawer)
        {
            if (!s_Caches.TryGetValue(type, out drawer))
            {
                Type drawerType = GetCustomPropertyDrawerType(type);
                drawer = (drawerType != null) ? (PropertyDrawer) Activator.CreateInstance(drawerType) : null;

                s_Caches.Add(type, drawer);
            }

            return (drawer != null);
        }

        static Type GetCustomPropertyDrawerType(Type type)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type drawerType in assembly.GetTypes())
                {
                    var customPropertyDrawerAttributes =
                        drawerType.GetCustomAttributes(typeof(CustomPropertyDrawer), true);
                    foreach (CustomPropertyDrawer customPropertyDrawer in customPropertyDrawerAttributes)
                    {
                        var field = customPropertyDrawer.GetType()
                            .GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (field != null)
                        {
                            var fieldType = field.GetValue(customPropertyDrawer) as Type;
                            if (fieldType != null && fieldType == type)
                            {
                                return drawerType;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}