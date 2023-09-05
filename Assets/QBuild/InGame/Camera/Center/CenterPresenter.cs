using System;
using QBuild.Behavior;
using QBuild.Stage;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;
using QBuild.Player.Controller;

namespace QBuild.Camera.Center
{
    public class CenterPresenter : IDisposable, IInitializable
    {
        private readonly CameraModel _cameraModel;
        private readonly CameraScriptableObject _cameraScriptableObject;
        private readonly CenterView _centerView;
        private readonly PlayerController _playerController;

        [Inject]
        public CenterPresenter(CameraModel cameraModel, StageScriptableObject stageScriptableObject,
            CameraScriptableObject cameraScriptableObject, CenterView centerView,PlayerController playerController)
        {
            _cameraModel = cameraModel;
            _cameraScriptableObject = cameraScriptableObject;
            _centerView = centerView;
            _playerController = playerController;
        }

        public void Dispose()
        {
            Object.Destroy(_centerView);
            _playerController.OnChangeGridPosition -= CameraMove;

        }

        public void Initialize()
        {
            _cameraModel.SetLookAt(_centerView.transform);
            _cameraScriptableObject.CenterOffset.Subscribe(
                offset => _centerView.transform.position = _centerView.GetCenterPosition() + offset
            );

            _playerController.OnChangeGridPosition += CameraMove;
        }
        
        private void CameraMove(Vector3 position)
        {
            var targetPosition = _centerView.transform.position;
            if (targetPosition == position) return;
            targetPosition.y = position.y;
            _centerView.GetComponent<GridMove>().MoveTo(targetPosition);
        }
    }
}