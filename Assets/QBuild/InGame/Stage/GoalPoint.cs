using SoVariableTool.ScriptableEvent;
using QBuild.Player.Controller;
using SoVariableTool;
using UnityEngine;

namespace QBuild.Stage
{
    public class GoalPoint : MonoBehaviour
    {
        [SerializeField] private UnitScriptableEventObject _goalEvent;
        
        private void Awake()
        {
            if (_goalEvent == null)
            {
                Debug.LogError("GoalEventが設定されていません", this);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController player))
            {
                _goalEvent.Raise();
            }
        }
    }
}