using System;
using QBuild.Gimmick.Effector;
using UnityEngine;

namespace QBuild.Gimmick.TriggerPattern
{
    public class CollisionTrigger : ITrigger
    {
        public event Action OnActive;
        public event Action OnDisable;


        public void TriggerBind(GimmickTrigger gimmick)
        {
            gimmick.OnEnter += OnEnter;
            gimmick.OnExit += OnExit;
        }

        private void OnEnter(BaseGimmick gimmick, Collider other)
        {
            BaseGimmickExtension.Execute(gimmick, new ContactEffectData() { Target = other.gameObject });
            OnActive?.Invoke();
        }

        private void OnExit(BaseGimmick gimmick, Collider other)
        {
            OnDisable?.Invoke();
        }
    }

    [DisallowMultipleComponent]
    public class ContactedEffector : MonoBehaviour, IEffector
    {
        public event Action<BaseEffectorData> OnContacted;

        public void Execute(BaseEffectorData e)
        {
            OnContacted?.Invoke(e);
        }
    }

    public class ContactEffectData : BaseEffectorData
    {
    }


    public static partial class BaseGimmickExtension
    {
        public static void OnContacted(this BaseGimmick gimmick, Action<BaseEffectorData> action)
        {
            var contactedEffector = GetOrAddComponent<ContactedEffector>(gimmick.gameObject);
            contactedEffector.OnContacted += action;
        }

        public static void Execute(BaseGimmick gimmick, BaseEffectorData data)
        {
            var contactedEffector = GetOrAddComponent<ContactedEffector>(gimmick.gameObject);
            contactedEffector.Execute(data);
        }
    }
}