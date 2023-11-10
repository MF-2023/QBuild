using System;
using UnityEngine;

namespace QBuild.Gimmick.TriggerPattern
{
    public interface ITrigger
    {
        public event Action OnActive;
        public event Action OnDisable;
        
        public void TriggerBind(GimmickTrigger gimmick);
    }

    public static partial class BaseGimmickExtension
    {
        private static T GetOrAddComponent<T>(GameObject gameObject)
            where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }
    }
}