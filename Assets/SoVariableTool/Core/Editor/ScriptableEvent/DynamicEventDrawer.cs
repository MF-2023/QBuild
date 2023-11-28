using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace SoVariableTool.ScriptableEvent
{
    [CustomPropertyDrawer(typeof(UnityEventDynamic), true)]
    public class DynamicEventDrawer : PropertyDrawer
    {
        protected class State
        {
            internal ReorderableList m_ReorderableList;
            public SerializedProperty property;
            public int lastSelectedIndex;
        }

        private static MethodInfo GeneratePopUpForType =
            typeof(UnityEventDrawer).GetMethod("GeneratePopUpForType", BindingFlags.Static | BindingFlags.NonPublic);

        private static MethodInfo GetEventParams =
            typeof(UnityEventDrawer).GetMethod("GetEventParams", BindingFlags.Static | BindingFlags.NonPublic);
    
        private static GUIStyle _foldoutHeader;

        private static float VerticalSpacing => EditorGUIUtility.standardVerticalSpacing;
        private static float Spacing => 3;

        private static readonly GUIContent DropdownIcon = EditorGUIUtility.IconContent("icon dropdown");
        private static readonly GUIContent MixedValueContent = EditorGUIUtility.TrTextContent("—", "Mixed Values");
        private static readonly GUIContent TempContent = new();

        private const string kNoFunctionString = "No Function";

        internal const string kInstancePath = "m_Target";
        internal const string kCallStatePath = "m_CallState";
        internal const string kArgumentsPath = "m_Arguments";
        internal const string kModePath = "m_Mode";
        internal const string kMethodNamePath = "m_MethodName";

        internal const string kFloatArgument = "m_FloatArgument";
        internal const string kIntArgument = "m_IntArgument";
        internal const string kObjectArgument = "m_ObjectArgument";
        internal const string kStringArgument = "m_StringArgument";
        internal const string kBoolArgument = "m_BoolArgument";
        internal const string kObjectArgumentAssemblyTypeName = "m_ObjectArgumentAssemblyTypeName";

        string m_Text;
        UnityEventBase m_DummyEvent;

        SerializedProperty m_Property;
        SerializedProperty m_BaseEventProperty;
        SerializedProperty m_ListenersArray;

        const int kExtraSpacing = 2;

        //State:
        ReorderableList m_ReorderableList;
        int m_LastSelectedIndex;
        State currentState;
        Dictionary<string, State> m_States = new Dictionary<string, State>();


        private State GetState(SerializedProperty prop)
        {
            var key = prop.propertyPath;
            m_States.Clear();
            m_States.TryGetValue(key, out var state);
            if (state?.m_ReorderableList.serializedProperty == null ||
                state.m_ReorderableList.serializedProperty.serializedObject != prop.serializedObject)
            {
                if (state == null)
                    state = new State();
                
                SerializedProperty listenersArray = prop.FindPropertyRelative("m_PersistentCalls.m_Calls");
                state.m_ReorderableList =
                    new ReorderableList(prop.serializedObject, listenersArray, true, true, true, true)
                    {
                        drawHeaderCallback = null,
                        drawFooterCallback = _ => { },
                        drawElementCallback = DrawEvent,
                        elementHeightCallback = OnGetElementHeight,
                        drawElementBackgroundCallback = DrawElementBackground,
                        onSelectCallback = OnSelectEvent,
                        onReorderCallback = OnReorderEvent,
                        onAddCallback = OnAddEvent,
                        onRemoveCallback = OnRemoveEvent,

                        headerHeight = 0,
                        footerHeight = 0,
                    };

                m_States[key] = state;
            }

            return state;
        }

        private static void DrawElementBackground(Rect rect, int index, bool active, bool focused)
        {
            var isPro = EditorGUIUtility.isProSkin;
            var color = GUI.color;

            focused = isPro && focused;

            ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, active, focused, true);
            GUI.color = color;
        }

        private State RestoreState(SerializedProperty property)
        {
            var state = GetState(property);

            m_ListenersArray = state.m_ReorderableList.serializedProperty;
            m_ReorderableList = state.m_ReorderableList;
            m_LastSelectedIndex = state.lastSelectedIndex;
            m_ReorderableList.index = m_LastSelectedIndex;

            return state;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            m_Property = property;

            m_BaseEventProperty = property.FindPropertyRelative("_unityEventBase");
            m_Text = label.text;

            currentState = RestoreState(m_BaseEventProperty);
            currentState.property = m_BaseEventProperty;

            OnGUI(position);
            currentState.lastSelectedIndex = m_LastSelectedIndex;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var eventProperty = property.FindPropertyRelative("_unityEventBase");
            RestoreState(eventProperty);

            float height = 0f;
            if (m_ReorderableList != null)
            {
                if (m_ReorderableList.serializedProperty is { isExpanded: false }) return EditorGUIUtility.singleLineHeight + VerticalSpacing + VerticalSpacing;

                height = m_ReorderableList.GetHeight();
                height += EditorGUIUtility.singleLineHeight;
            }

            return height + VerticalSpacing;
        }

        private void OnGUI(Rect rect)
        {

            if (m_ListenersArray is not { isArray: true })
                return;


            m_DummyEvent = Activator.CreateInstance(m_BaseEventProperty.managedReferenceValue.GetType()) as UnityEventBase;//GetDummyEvent.Invoke(null, new object[] { m_BaseEventProperty }) as UnityEventBase;

            if (m_DummyEvent == null)
                return;


            if (m_ReorderableList == null) return;

            if (ReorderableList.defaultBehaviours == null)
                m_ReorderableList.DoList(Rect.zero);

            var oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            rect.xMin += 8 * oldIndent;

            var headerRect = new Rect(rect.x, rect.y, rect.width, 18);
            var listRect = new Rect(rect) { yMin = headerRect.yMax };

            ReorderableList.defaultBehaviours.DrawHeaderBackground(headerRect);
            var isExpanded = DrawListHeader(headerRect, m_ReorderableList);

            if (isExpanded)
            {
                ReorderableList.defaultBehaviours.draggingHandle.fixedWidth = 6;

                m_ReorderableList.DoList(listRect);

                ReorderableList.defaultBehaviours.draggingHandle.fixedWidth = 0;
            }

            EditorGUI.indentLevel = oldIndent;
        }

        protected virtual bool DrawListHeader(Rect rect, ReorderableList list)
        {
            const int sizeWidth = 24;
            const int buttonsWidth = 54;

            var property = list.serializedProperty;

            rect.xMin += 16;
            rect.yMin += 1;
            rect.height = EditorGUIUtility.singleLineHeight;

            var foldoutRect = new Rect(rect);
            foldoutRect.width -= buttonsWidth + sizeWidth;
            foldoutRect.height -= 1;

            if (_foldoutHeader == null)
                _foldoutHeader = new GUIStyle(EditorStyles.foldoutHeader)
                {
                    richText = true, fontStyle = FontStyle.Normal, clipping = TextClipping.Clip,
                    fixedHeight = 0, padding = new RectOffset(14, 5, 2, 2),
                };

            // Header
            {
                var eventParams = (string)GetEventParams.Invoke(null, new[] { m_DummyEvent });
                var hex = EditorGUIUtility.isProSkin ? "ffffff" : "000000";
                var text = (string.IsNullOrEmpty(m_Text) ? "Event" : m_Text) + $"<color=#{hex}70>{eventParams}</color>";

                property.isExpanded =
                    EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, text, _foldoutHeader);
                EditorGUI.EndFoldoutHeaderGroup();
            }

            var sizeRect = new Rect(rect) { x = foldoutRect.xMax, width = sizeWidth };
            sizeRect.yMin += 1;
            sizeRect.height -= 1;

            // Size field
            {
                EditorGUI.BeginChangeCheck();
                var numberField = EditorStyles.numberField;
                numberField.contentOffset = new Vector2(0, -1);

                var arraySize = EditorGUI.IntField(sizeRect, property.arraySize);

                numberField.contentOffset = Vector2.zero;
                if (EditorGUI.EndChangeCheck())
                    property.arraySize = arraySize;
            }

            var footerRect = new Rect(rect) { x = sizeRect.xMax + 12, width = buttonsWidth };
            footerRect.yMin += 1;

            // Footer buttons
            {
                var footerBg = ReorderableList.defaultBehaviours.footerBackground;
                footerBg.fixedHeight = 0.01f;

                ReorderableList.defaultBehaviours.DrawFooter(footerRect, list);

                footerBg.fixedHeight = 0;
            }

            return property.isExpanded;
        }

        static PersistentListenerMode GetMode(SerializedProperty mode)
        {
            return (PersistentListenerMode)mode.enumValueIndex;
        }

        float OnGetElementHeight(int index)
        {
            if (m_ReorderableList == null)
                return 0;

            var element = m_ListenersArray.GetArrayElementAtIndex(index);

            var mode = element.FindPropertyRelative(kModePath);
            var modeEnum = GetMode(mode);

            var spacing = VerticalSpacing + kExtraSpacing;

            if (modeEnum == PersistentListenerMode.Object || (modeEnum != PersistentListenerMode.Void &&
                                                              modeEnum != PersistentListenerMode.EventDefined))
                return EditorGUIUtility.singleLineHeight * 2 + VerticalSpacing + spacing;

            return EditorGUIUtility.singleLineHeight + spacing;
        }

        protected virtual void DrawEvent(Rect rect, int index, bool isActive, bool isFocused)
        {
            var pListener = m_ListenersArray.GetArrayElementAtIndex(index);

            var contentRect = rect;
            contentRect.xMin -= 6;
            contentRect.xMax += 2;
            contentRect.y += 1;

            Rect[] subRects = GetRowRects(contentRect);
            Rect enabledRect = subRects[0];
            Rect goRect = subRects[1];
            Rect functionRect = subRects[2];
            Rect argRect = subRects[3];

            // find the current event target...
            var callState = pListener.FindPropertyRelative(kCallStatePath);
            var mode = pListener.FindPropertyRelative(kModePath);
            var arguments = pListener.FindPropertyRelative(kArgumentsPath);
            var listenerTarget = pListener.FindPropertyRelative(kInstancePath);
            var methodName = pListener.FindPropertyRelative(kMethodNamePath);

            Color c = GUI.backgroundColor;
            GUI.backgroundColor = Color.white;

            var callStateEnum = (UnityEventCallState)callState.enumValueIndex;
            var isEditorAndRuntime = callStateEnum == UnityEventCallState.EditorAndRuntime;
            var isRuntime = callStateEnum == UnityEventCallState.RuntimeOnly;

            var toggleRect = enabledRect;
            toggleRect.width = 16;

            if (isEditorAndRuntime || (isRuntime && Application.isPlaying))
            {
                var markRect = new Rect(rect) { width = 2 };
                markRect.x -= 20;
                EditorGUI.DrawRect(markRect, new Color(1, 0.7f, 0.4f, 1));
            }

            var evt = Event.current;
            var color = GUI.color;
            var mousePos = evt.mousePosition;
            {
                var isHover = toggleRect.Contains(mousePos);
                if (isHover)
                {
                    // Ooh, these beautiful 2-pixels of rounded edges..
                    GUI.DrawTexture(toggleRect, Texture2D.whiteTexture, ScaleMode.ScaleToFit, true, 1,
                        new Color(1, 1, 1, 0.15f), Vector4.zero, 2);
                }
            }

            GUI.color = new Color(1, 1, 1, 0.75f);
            GUI.Box(toggleRect, DropdownIcon, EditorStyles.centeredGreyMiniLabel);
            GUI.color = color;

            GUI.color = new Color(0, 0, 0, 0);
            EditorGUI.PropertyField(toggleRect, callState, GUIContent.none);
            GUI.color = color;


            var isOff = callStateEnum == UnityEventCallState.Off;
            EditorGUI.BeginDisabledGroup(isOff);

            EditorGUI.BeginChangeCheck();
            {
                GUI.Box(goRect, GUIContent.none);
                EditorGUI.PropertyField(goRect, listenerTarget, GUIContent.none);
                if (EditorGUI.EndChangeCheck())
                    methodName.stringValue = null;
            }

            SerializedProperty argument;
            var modeEnum = GetMode(mode);
            //only allow argument if we have a valid target / method
            if (listenerTarget.objectReferenceValue == null || string.IsNullOrEmpty(methodName.stringValue))
                modeEnum = PersistentListenerMode.Void;

            switch (modeEnum)
            {
                case PersistentListenerMode.Float:
                    argument = arguments.FindPropertyRelative(kFloatArgument);
                    break;
                case PersistentListenerMode.Int:
                    argument = arguments.FindPropertyRelative(kIntArgument);
                    break;
                case PersistentListenerMode.Object:
                    argument = arguments.FindPropertyRelative(kObjectArgument);
                    break;
                case PersistentListenerMode.String:
                    argument = arguments.FindPropertyRelative(kStringArgument);
                    break;
                case PersistentListenerMode.Bool:
                    argument = arguments.FindPropertyRelative(kBoolArgument);
                    break;
                default:
                    argument = arguments.FindPropertyRelative(kIntArgument);
                    break;
            }

            var desiredArgTypeName = arguments.FindPropertyRelative(kObjectArgumentAssemblyTypeName).stringValue;
            var desiredType = typeof(Object);
            if (!string.IsNullOrEmpty(desiredArgTypeName))
                desiredType = Type.GetType(desiredArgTypeName, false) ?? typeof(Object);


            argRect.xMin = goRect.xMax + Spacing;

            if (modeEnum == PersistentListenerMode.Object)
            {
                EditorGUI.BeginChangeCheck();
                var result = EditorGUI.ObjectField(argRect, GUIContent.none, argument.objectReferenceValue, desiredType,
                    true);
                if (EditorGUI.EndChangeCheck())
                    argument.objectReferenceValue = result;
            }
            else if (modeEnum != PersistentListenerMode.Void && modeEnum != PersistentListenerMode.EventDefined)
            {
                EditorGUI.PropertyField(argRect, argument, GUIContent.none);
            }

            using (new EditorGUI.DisabledScope(listenerTarget.objectReferenceValue == null))
            {
                EditorGUI.BeginProperty(functionRect, GUIContent.none, methodName);
                {
                    GUIContent buttonContent;
                    if (EditorGUI.showMixedValue)
                    {
                        buttonContent = MixedValueContent;
                    }
                    else
                    {
                        var buttonLabel = new StringBuilder();
                        if (listenerTarget.objectReferenceValue == null || string.IsNullOrEmpty(methodName.stringValue))
                        {
                            buttonLabel.Append(kNoFunctionString);
                        }
                        else if (!UnityEventDrawer.IsPersistantListenerValid(m_DummyEvent, methodName.stringValue,
                                     listenerTarget.objectReferenceValue, GetMode(mode), desiredType))
                        {
                            var instanceString = "UnknownComponent";
                            var instance = listenerTarget.objectReferenceValue;
                            if (instance != null)
                                instanceString = instance.GetType().Name;

                            buttonLabel.Append(string.Format("<Missing {0}.{1}>", instanceString, methodName.stringValue));
                        }
                        else
                        {
                            buttonLabel.Append(listenerTarget.objectReferenceValue.GetType().Name);

                            if (!string.IsNullOrEmpty(methodName.stringValue))
                            {
                                buttonLabel.Append(".");
                                if (methodName.stringValue.StartsWith("set_"))
                                    buttonLabel.Append(methodName.stringValue.Substring(4));
                                else
                                    buttonLabel.Append(methodName.stringValue);
                            }
                        }

                        TempContent.text = buttonLabel.ToString();
                        buttonContent = TempContent;
                    }

                    if (GUI.Button(functionRect, buttonContent, EditorStyles.popup))
                    {
                        var delegateType = m_DummyEvent.GetType();

                        var delegateMethod = delegateType.GetMethod("Invoke");
                        var delegateArgumentsTypes = delegateMethod.GetParameters().Select(x => x.ParameterType).ToArray();
                    
                        var popup = BuildPopupList(listenerTarget.objectReferenceValue, m_DummyEvent, pListener,
                            delegateArgumentsTypes);
                        popup.DropDown(functionRect);
                    }
                }
                EditorGUI.EndProperty();
            }

            EditorGUI.EndDisabledGroup();
            GUI.backgroundColor = c;
        }

        Rect[] GetRowRects(Rect rect)
        {
            Rect[] rects = new Rect[4];

            rect.height = EditorGUIUtility.singleLineHeight;
            rect.y += 2;

            Rect enabledRect = rect;
            enabledRect.width = 16 + Spacing - 1;

            Rect goRect = rect;
            goRect.xMin = enabledRect.xMax;
            goRect.width = rect.width;
            // Shrink object field when inspector is small
            goRect.width *= Mathf.Lerp(0, 0.4f, (rect.width - 125) / (350 - 100));
            goRect.width = Mathf.Max(goRect.width, 35);

            Rect functionRect = rect;
            functionRect.xMin = goRect.xMax + Spacing;

            Rect argRect = rect;
            argRect.y += EditorGUIUtility.singleLineHeight + VerticalSpacing;

            rects[0] = enabledRect;
            rects[1] = goRect;
            rects[2] = functionRect;
            rects[3] = argRect;
            return rects;
        }

        protected virtual void OnRemoveEvent(ReorderableList list)
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
            m_LastSelectedIndex = list.index;
        }

        protected virtual void OnAddEvent(ReorderableList list)
        {
            if (m_ListenersArray.hasMultipleDifferentValues)
            {
                //When increasing a multi-selection array using Serialized Property
                //Data can be overwritten if there is mixed values.
                //The Serialization system applies the Serialized data of one object, to all other objects in the selection.
                //We handle this case here, by creating a SerializedObject for each object.
                //Case 639025.
                foreach (var targetObject in m_ListenersArray.serializedObject.targetObjects)
                {
                    using (var temSerialziedObject = new SerializedObject(targetObject))
                    {
                        var listenerArrayProperty = temSerialziedObject.FindProperty(m_ListenersArray.propertyPath);
                        listenerArrayProperty.arraySize += 1;
                        temSerialziedObject.ApplyModifiedProperties();
                    }
                }

                m_ListenersArray.serializedObject.SetIsDifferentCacheDirty();
                m_ListenersArray.serializedObject.Update();
                list.index = list.serializedProperty.arraySize - 1;
            }
            else
            {
                ReorderableList.defaultBehaviours.DoAddButton(list);
            }

            m_LastSelectedIndex = list.index;
            var pListener = m_ListenersArray.GetArrayElementAtIndex(list.index);

            var callState = pListener.FindPropertyRelative(kCallStatePath);
            var listenerTarget = pListener.FindPropertyRelative(kInstancePath);
            var methodName = pListener.FindPropertyRelative(kMethodNamePath);
            var mode = pListener.FindPropertyRelative(kModePath);
            var arguments = pListener.FindPropertyRelative(kArgumentsPath);

            callState.enumValueIndex = (int)UnityEventCallState.RuntimeOnly;
            listenerTarget.objectReferenceValue = null;
            methodName.stringValue = null;
            mode.enumValueIndex = (int)PersistentListenerMode.Void;
            arguments.FindPropertyRelative(kFloatArgument).floatValue = 0;
            arguments.FindPropertyRelative(kIntArgument).intValue = 0;
            arguments.FindPropertyRelative(kObjectArgument).objectReferenceValue = null;
            arguments.FindPropertyRelative(kStringArgument).stringValue = null;
            arguments.FindPropertyRelative(kObjectArgumentAssemblyTypeName).stringValue = null;
        }

        protected virtual void OnSelectEvent(ReorderableList list)
        {
            m_LastSelectedIndex = list.index;
        }

        protected virtual void OnReorderEvent(ReorderableList list)
        {
            m_LastSelectedIndex = list.index;
        }

        static void ClearEventFunction(object source) => ((UnityEventFunction)source).Clear();

        private static GenericMenu BuildPopupList(
            UnityEngine.Object target,
            UnityEventBase dummyEvent,
            SerializedProperty listener,
            System.Type[] delegateArgumentsTypes
        )
        {
            var target1 = target;
            if (target1 is Component)
                target1 = (UnityEngine.Object)(target as Component).gameObject;
            var propertyRelative = listener.FindPropertyRelative("m_MethodName");
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("No Function"), string.IsNullOrEmpty(propertyRelative.stringValue),
                ClearEventFunction,
                (object)new UnityEventFunction(listener, (UnityEngine.Object)null, (MethodInfo)null,
                    PersistentListenerMode.EventDefined));
            if (target1 == (UnityEngine.Object)null)
                return menu;
            menu.AddSeparator("");
            var toRelease1 = CollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Get();
            var toRelease2 = CollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Get();

            GeneratePopUpForType.Invoke(null,
                new object[] { menu, target1, target1.GetType().Name, listener, delegateArgumentsTypes });

            toRelease1[target1.GetType().Name] = 0;
            if (target1 is not GameObject gameObject) return menu;
            var components = gameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                if ((UnityEngine.Object)component == (UnityEngine.Object)null) continue;
                if (toRelease1.TryGetValue(component.GetType().Name, out var num))
                    ++num;
                toRelease1[component.GetType().Name] = num;
            }

            foreach (var target2 in components)
            {
                if (target2 == (UnityEngine.Object)null) continue;
                var type = target2.GetType();
                var targetName = type.Name;
                var num = 0;
                if (toRelease1[type.Name] > 0)
                    targetName = !toRelease2.TryGetValue(type.FullName, out num)
                        ? type.FullName
                        : $"{type.FullName} ({num})";
                GeneratePopUpForType.Invoke(null, new object[]
                {
                    menu, target2, targetName, listener,
                    delegateArgumentsTypes
                });
                if (type.FullName != null) toRelease2[type.FullName] = num + 1;
            }

            CollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Release(toRelease1);
            CollectionPool<Dictionary<string, int>, KeyValuePair<string, int>>.Release(toRelease2);

            return menu;
        }

        private struct UnityEventFunction
        {
            private readonly SerializedProperty m_Listener;
            private readonly UnityEngine.Object m_Target;
            private readonly MethodInfo m_Method;
            private readonly PersistentListenerMode m_Mode;

            public UnityEventFunction(
                SerializedProperty listener,
                UnityEngine.Object target,
                MethodInfo method,
                PersistentListenerMode mode)
            {
                this.m_Listener = listener;
                this.m_Target = target;
                this.m_Method = method;
                this.m_Mode = mode;
            }

            public void Assign()
            {
                SerializedProperty propertyRelative1 = this.m_Listener.FindPropertyRelative("m_Target");
                SerializedProperty propertyRelative2 = this.m_Listener.FindPropertyRelative("m_TargetAssemblyTypeName");
                SerializedProperty propertyRelative3 = this.m_Listener.FindPropertyRelative("m_MethodName");
                SerializedProperty propertyRelative4 = this.m_Listener.FindPropertyRelative("m_Mode");
                SerializedProperty propertyRelative5 = this.m_Listener.FindPropertyRelative("m_Arguments");
                propertyRelative1.objectReferenceValue = this.m_Target;
                propertyRelative2.stringValue = this.m_Method.DeclaringType.AssemblyQualifiedName;
                propertyRelative3.stringValue = this.m_Method.Name;
                propertyRelative4.enumValueIndex = (int)this.m_Mode;
                if (this.m_Mode == PersistentListenerMode.Object)
                {
                    SerializedProperty propertyRelative6 =
                        propertyRelative5.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
                    System.Reflection.ParameterInfo[] parameters = this.m_Method.GetParameters();
                    int num = parameters.Length != 1
                        ? 0
                        : (typeof(UnityEngine.Object).IsAssignableFrom(parameters[0].ParameterType) ? 1 : 0);
                    propertyRelative6.stringValue = num == 0
                        ? typeof(UnityEngine.Object).AssemblyQualifiedName
                        : parameters[0].ParameterType.AssemblyQualifiedName;
                }

                this.ValidateObjectParamater(propertyRelative5, this.m_Mode);
                this.m_Listener.serializedObject.ApplyModifiedProperties();
            }

            private void ValidateObjectParamater(
                SerializedProperty arguments,
                PersistentListenerMode mode)
            {
                SerializedProperty propertyRelative1 = arguments.FindPropertyRelative("m_ObjectArgumentAssemblyTypeName");
                SerializedProperty propertyRelative2 = arguments.FindPropertyRelative("m_ObjectArgument");
                UnityEngine.Object objectReferenceValue = propertyRelative2.objectReferenceValue;
                if (mode != PersistentListenerMode.Object)
                {
                    propertyRelative1.stringValue = typeof(UnityEngine.Object).AssemblyQualifiedName;
                    propertyRelative2.objectReferenceValue = (UnityEngine.Object)null;
                }
                else
                {
                    if (objectReferenceValue == (UnityEngine.Object)null)
                        return;
                    System.Type type = System.Type.GetType(propertyRelative1.stringValue, false);
                    if (typeof(UnityEngine.Object).IsAssignableFrom(type) &&
                        type.IsInstanceOfType((object)objectReferenceValue))
                        return;
                    propertyRelative2.objectReferenceValue = (UnityEngine.Object)null;
                }
            }

            public void Clear()
            {
                this.m_Listener.FindPropertyRelative("m_MethodName").stringValue = (string)null;
                this.m_Listener.FindPropertyRelative("m_Mode").enumValueIndex = 1;
                this.m_Listener.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}