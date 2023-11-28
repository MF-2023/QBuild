using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using SoVariableTool.Binding;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SoVariableTool.Bind
{
    [CustomPropertyDrawer(typeof(BindingValue))]
    public class BindingValueDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            container.Add(CreateObjectField(property));

            container.Add(CreateVariableNameElement(property));

            return container;
        }

        private static VisualElement CreateObjectField(SerializedProperty property)
        {
            var targetObject = property.FindPropertyRelative("_targetObject");
            var targetObjectField = new ObjectField("Target Object");
            targetObjectField.BindProperty(targetObject);
            return targetObjectField;
        }

        private static VisualElement CreateVariableNameElement(SerializedProperty property)
        {
            var variableName = property.FindPropertyRelative("_variableName");
            var variableNameField = new TextField();
            variableNameField.BindProperty(variableName);
            variableNameField.SetEnabled(false);

            var bindButton = new Button
            {
                text = "Bind",
            };

            bindButton.clicked += () =>
            {
                var value = GetTargetObjectOfProperty(property) as BindingValue;
                var targetObject = property.FindPropertyRelative("_targetObject");
                var dropdown =
                    new TypeMemberAdvancedDropdown(new AdvancedDropdownState(), targetObject.objectReferenceValue);
                dropdown.OnSelected += item => { value.SetBind(item.Target, item.name, item.VariableType); };
                dropdown.Show(new Rect(bindButton.worldBound.position, bindButton.worldBound.size));
            };

            var variableNameElement = new VisualElement();
            variableNameElement.name = "Row";
            variableNameElement.style.flexDirection = FlexDirection.Row;
            variableNameElement.style.flexGrow = 1;

            variableNameElement.Add(bindButton);
            variableNameElement.Add(variableNameField);

            return variableNameElement;
        }

        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            if (prop == null) return null;

            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach (string element in elements)
            {
                if (element.Contains("["))
                {
                    string elementName = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));
                    int index = Convert.ToInt32(element.Substring(element.IndexOf("[", StringComparison.Ordinal))
                        .Replace("[", "").Replace("]", ""));
                    obj = GetValue_Imp(obj, elementName, index);
                }
                else
                {
                    obj = GetValue_Imp(obj, element);
                }
            }

            return obj;
        }

        private static object GetValue_Imp(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();

            while (type != null)
            {
                var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (f != null)
                    return f.GetValue(source);

                var p = type.GetProperty(name,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p != null)
                    return p.GetValue(source, null);

                type = type.BaseType;
            }

            return null;
        }

        private static object GetValue_Imp(object source, string name, int index)
        {
            var enumerable = GetValue_Imp(source, name) as IEnumerable;
            if (enumerable == null) return null;
            var enm = enumerable.GetEnumerator();
            //while (index-- >= 0)
            //    enm.MoveNext();
            //return enm.Current;

            for (int i = 0; i <= index; i++)
            {
                if (!enm.MoveNext()) return null;
            }

            return enm.Current;
        }
    }

    public class TypeMemberAdvancedDropdown : AdvancedDropdown
    {
        public event Action<TypeMemberItem> OnSelected;

        public TypeMemberAdvancedDropdown(AdvancedDropdownState state, Object gameObject) : base(state)
        {
            var minSize = minimumSize;
            minSize.y = 200;
            minimumSize = minSize;
            _object = gameObject;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(_object.name);

            GameObject gameObject = null;
            switch (_object)
            {
                case GameObject o:
                    gameObject = o;
                    break;
                case Component component:
                    gameObject = component.gameObject;
                    break;
                case ScriptableObject:
                    break;
            }

            if (gameObject == null) return root;

            var components = gameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                var componentItem = new AdvancedDropdownItem(component.GetType().Name);
                // SerializeFieldとpublicなフィールドを取得
                var fields = component.GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public |
                               BindingFlags.NonPublic)
                    .Where(x =>
                        x.GetCustomAttribute<SerializeField>() != null || x.IsPublic);

                // フィールドとプロパティの名前を昇順で並べて結合
                foreach (var field in fields)
                {
                    Debug.Log(field.Name);
                    componentItem.AddChild(new TypeMemberItem(field.Name, component, MemberVariableType.Field));
                }

                // Propertyも取得
                var properties = component.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public |
                                   BindingFlags.NonPublic)
                    .Where(x =>
                        x.GetCustomAttribute<SerializeField>() != null || x.CanRead && x.CanWrite);

                foreach (var property in properties)
                {
                    componentItem.AddChild(new TypeMemberItem(property.Name, component, MemberVariableType.Property));
                }

                root.AddChild(componentItem);
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is TypeMemberItem typeMemberItem)
            {
                OnSelected?.Invoke(typeMemberItem);
            }
        }

        private readonly Object _object;

        public class TypeMemberItem : AdvancedDropdownItem
        {
            public TypeMemberItem(string name, Object target, MemberVariableType variableType) : base(name)
            {
                VariableType = variableType;
                Target = target;
            }

            public MemberVariableType VariableType { get; private set; }
            public Object Target { get; private set; }
        }
    }
}