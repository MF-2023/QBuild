using System;
using QBuild.Stage;
using QBuild.Utilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace QBuild.Camera
{
    public class CameraPresenter : IDisposable, IInitializable
    {
        [Inject]
        public CameraPresenter(CameraModel cameraModel, CircleCamera cameraView, CameraInput cameraInput,
            StageScriptableObject stageScriptableObject)
        {
            _cameraModel = cameraModel;
            _cameraView = cameraView;
            _cameraInput = cameraInput;
            _stageScriptableObject = stageScriptableObject;
        }

        public void Initialize()
        {
            _cameraInput.OnCameraMove += CameraMove;
            _cameraModel.OnCameraDirectionChanged += CameraTurn;
            _cameraModel.OnLookAtChanged += _cameraView.SetLookAt;
            
            _center = new GameObject("Center");
            var centerPos = new Vector3(_stageScriptableObject.Width / 2, 0, _stageScriptableObject.Depth / 2);
            _center.transform.position = centerPos;
            _cameraModel.SetLookAt(_center.transform);
            _cameraModel.SetCameraDirection(Direction.East);
        }

        public void Dispose()
        {
            _cameraInput.OnCameraMove -= CameraMove;
            _cameraModel.OnCameraDirectionChanged -= CameraTurn;
            _cameraModel.OnLookAtChanged -= _cameraView.SetLookAt;

            Object.Destroy(_center);
        }

        private void CameraTurn(Direction direction)
        {
            _cameraView.SetCameraIndex((int)direction);
        }
        private void CameraMove(int index)
        {
            switch (index)
            {
                case 1:
                    _cameraModel.TurnRight();
                    break;
                case -1:
                    _cameraModel.TurnLeft();
                    break;
            }
        }

        private readonly CameraModel _cameraModel;
        private readonly CircleCamera _cameraView;
        private readonly CameraInput _cameraInput;
        private readonly StageScriptableObject _stageScriptableObject;
        private GameObject _center;
    }
}