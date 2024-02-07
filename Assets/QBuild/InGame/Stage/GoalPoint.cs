using QBuild.Player.Controller;
using QBuild.Result;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerController player))
            {
                player.Goal();
                _goalEvent.Raise();
                //_goalEvent.Raise();
            }
        }
    }
}