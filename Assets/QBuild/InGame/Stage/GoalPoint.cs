using Cinemachine;
using QBuild.Player.Controller;
using QBuild.Result;
using SoVariableTool;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

namespace QBuild.Stage
{
    public class GoalPoint : MonoBehaviour
    {
        [SerializeField] private UnitScriptableEventObject _goalEvent;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private Transform _goalCenterPoint;
        
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
                
                //プレイヤーの位置と向きをgoalCenterPointに合わせる
                player.transform.DOMove(_goalCenterPoint.position, 2.0f);
                
                //ぷれいやーのRotationをgoalCenterPointと同じにする
                player.transform.DORotate(_goalCenterPoint.rotation.eulerAngles, 2.0f);
                
                _goalEvent.Raise();
            }
        }
    }
}