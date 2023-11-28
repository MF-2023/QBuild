using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Events;

namespace SoVariableTool
{
    [Serializable]
    public class UnityEventDynamic
    {
        [SerializeReference] public UnityEventBase _unityEventBase = new UnityEvent();

        public void Invoke(object[] args)
        {
            _invokeAction.Invoke(_unityEventBase, args);
        }

        
        private static Action<UnityEventBase, object[]> _invokeAction =
            CreateMethodCallingMethod(typeof(UnityEventBase), "Invoke", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Action<UnityEventBase, object[]> CreateMethodCallingMethod(Type classType, string methodName,
            BindingFlags bindingFlags)
        {
            var selfPramType = Expression.Parameter(classType, "self");

            var targetPramType = Expression.Parameter(typeof(object[]), "args");
            var methodInfo = classType.GetMethod(methodName, bindingFlags);

            var lambda =
                Expression.Lambda<Action<UnityEventBase, object[]>>(
                    Expression.Call(
                        selfPramType,
                        methodInfo,
                        targetPramType), selfPramType, targetPramType
                );

            return lambda.Compile();
        }
    }
}