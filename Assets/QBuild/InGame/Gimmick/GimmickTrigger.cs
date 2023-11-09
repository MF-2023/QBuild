using System;
using QBuild.Gimmick.Effector;
using QBuild.Gimmick.TriggerPattern;
using SherbetInspector.Core.Attributes;
using UnityEngine;

namespace QBuild.Gimmick
{
    public class GimmickTrigger : MonoBehaviour
    {
        [Tooltip("動かすギミック")] [SerializeField] private BaseGimmick _gimmick;

        [SerializeReference, SubclassSelector] private ITrigger _baseTrigger;
        
        
        
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;
        private void Awake()
        {
            if (_baseTrigger == null)
            {
                Debug.LogError("Trigger が設定されていません。", this);
                return;
            }

            if (_gimmick == null)
            {
                Debug.LogError("対象のGimmick が設定されていません。", this);
                return;
            }

            _baseTrigger.OnActive += _gimmick.Active;
            _baseTrigger.OnDisable += _gimmick.Disable;
            
            _baseTrigger.TriggerBind(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            
            OnEnter?.Invoke(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            OnExit?.Invoke(other);
        }
    }
}