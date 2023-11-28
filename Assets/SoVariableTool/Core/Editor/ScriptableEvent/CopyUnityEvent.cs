using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace SoVariableTool.ScriptableEvent
{
    public static class CopyUnityEvent
    {
        public static void CopyUnityEvents(SerializedProperty sourceObj, UnityEventBase dest, bool debug = false)
        {
            var persistentCalls =
                sourceObj.FindPropertyRelative("m_PersistentCalls.m_Calls");
            for (var i = 0; i < persistentCalls.arraySize; ++i)
            {
                var target = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_Target")
                    .objectReferenceValue;
                var methodName = persistentCalls.GetArrayElementAtIndex(i).FindPropertyRelative("m_MethodName")
                    .stringValue;
                MethodInfo method = null;
                try
                {
                    method = target.GetType().GetMethod(methodName,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                catch
                {
                    foreach (var info in target.GetType()
                                 .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                 .Where(x => x.Name == methodName))
                    {
                        var targetParameters = info.GetParameters();
                        if (targetParameters.Length < 2)
                        {
                            method = info;
                        }
                    }
                }

                if(method == null) continue;
                var parameters = method.GetParameters();
                var delegateMethod = dest.GetType().GetMethod("Invoke");
                var delegateArgumentsTypes = delegateMethod?.GetParameters().Select(x => x.ParameterType).ToArray();

                if (delegateArgumentsTypes!.Length > 0 && delegateArgumentsTypes[0] == parameters[0].ParameterType)
                {
                    var addNoParameterPersistentListener =
                        AddNoParameterPersistentListenerTable[parameters[0].ParameterType] as
                            Action<object, Object, string>;
                    addNoParameterPersistentListener?.Invoke(dest, target, methodName);
                }
                else
                {
                    var addPersistentListener = AddPersistentListenerTable[parameters[0].ParameterType] as
                        Action<object, Object, string, SerializedProperty>;
                    addPersistentListener?.Invoke(dest, target, methodName, persistentCalls.GetArrayElementAtIndex(i));
                }
            }
        }

        #region AddPersistentListener

        private static Hashtable CreateAddNoParameterPersistentListenerTable()
        {
            var table = new Hashtable
            {
                [typeof(bool)] = new Action<object, Object, string>(AddNoParameterPersistentListener<bool>),
                [typeof(int)] = new Action<object, Object, string>(AddNoParameterPersistentListener<int>),
                [typeof(float)] = new Action<object, Object, string>(AddNoParameterPersistentListener<float>),
                [typeof(string)] = new Action<object, Object, string>(AddNoParameterPersistentListener<string>),
                [typeof(Object)] = new Action<object, Object, string>(AddNoParameterPersistentListener<Object>),
            };

            return table;
        }

        private static Hashtable CreateAddPersistentListenerTable()
        {
            var table = new Hashtable
            {
                [typeof(bool)] = new Action<object, Object, string, SerializedProperty>(AddBoolPersistentListener),
                [typeof(int)] = new Action<object, Object, string, SerializedProperty>(AddIntPersistentListener),
                [typeof(float)] = new Action<object, Object, string, SerializedProperty>(AddFloatPersistentListener),
                [typeof(string)] = new Action<object, Object, string, SerializedProperty>(AddStringPersistentListener),
                [typeof(Object)] = new Action<object, Object, string, SerializedProperty>(AddObjectPersistentListener)
            };

            return table;
        }

        private static void AddNoParameterPersistentListener<T>(object unityEventBase, Object target, string methodName)
        {
            var execute = Delegate.CreateDelegate(typeof(UnityAction<T>), target, methodName) as UnityAction<T>;
            UnityEventTools.AddPersistentListener(
                unityEventBase as UnityEvent<T>,
                execute);
        }

        private static void AddBoolPersistentListener(object unityEventBase, Object target, string methodName,
            SerializedProperty defaultValueProperty)
        {
            var defaultValue = defaultValueProperty
                .FindPropertyRelative("m_Arguments.m_BoolArgument").boolValue;
            var execute = Delegate.CreateDelegate(typeof(UnityAction<bool>), target, methodName) as UnityAction<bool>;
            UnityEventTools.AddBoolPersistentListener(
                unityEventBase as UnityEventBase,
                execute,
                defaultValue
            );
        }

        private static void AddIntPersistentListener(object unityEventBase, Object target, string methodName,
            SerializedProperty defaultValueProperty)
        {
            var defaultValue = defaultValueProperty
                .FindPropertyRelative("m_Arguments.m_IntArgument").intValue;
            var execute = Delegate.CreateDelegate(typeof(UnityAction<int>), target, methodName) as UnityAction<int>;
            UnityEventTools.AddIntPersistentListener(
                unityEventBase as UnityEventBase,
                execute,
                defaultValue
            );
        }

        private static void AddFloatPersistentListener(object unityEventBase, Object target, string methodName,
            SerializedProperty defaultValueProperty)
        {
            var defaultValue = defaultValueProperty
                .FindPropertyRelative("m_Arguments.m_FloatArgument").floatValue;
            var execute = Delegate.CreateDelegate(typeof(UnityAction<float>), target, methodName) as UnityAction<float>;
            UnityEventTools.AddFloatPersistentListener(
                unityEventBase as UnityEventBase,
                execute,
                defaultValue
            );
        }

        private static void AddStringPersistentListener(object unityEventBase, Object target, string methodName,
            SerializedProperty defaultValueProperty)
        {
            var defaultValue = defaultValueProperty
                .FindPropertyRelative("m_Arguments.m_StringArgument").stringValue;
            var execute =
                Delegate.CreateDelegate(typeof(UnityAction<string>), target, methodName) as UnityAction<string>;
            UnityEventTools.AddStringPersistentListener(
                unityEventBase as UnityEventBase,
                execute,
                defaultValue
            );
        }

        private static void AddObjectPersistentListener(object unityEventBase, Object target, string methodName,
            SerializedProperty defaultValueProperty)
        {
            var defaultValue = defaultValueProperty
                .FindPropertyRelative("m_Arguments.m_ObjectArgument").objectReferenceValue;
            var execute =
                Delegate.CreateDelegate(typeof(UnityAction<Object>), target, methodName) as UnityAction<Object>;
            UnityEventTools.AddObjectPersistentListener(
                unityEventBase as UnityEventBase,
                execute,
                defaultValue
            );
        }

        #endregion

        private static readonly Hashtable AddNoParameterPersistentListenerTable =
            CreateAddNoParameterPersistentListenerTable();

        private static readonly Hashtable AddPersistentListenerTable = CreateAddPersistentListenerTable();
    }
}