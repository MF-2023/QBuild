using Cinemachine;
using QBuild.Player.Controller;
using QBuild.Result;
using SoVariableTool;
using UnityEngine;
using UnityEngine.Playables;

namespace QBuild.Stage
{
    public class GoalPoint : MonoBehaviour
    {
        [SerializeField] private UnitScriptableEventObject _goalEvent;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        
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
                _virtualCamera.Priority = 15;
                //player.Goal();
                _goalEvent.Raise();
                //_goalEvent.Raise();
            }
        }
    }
}