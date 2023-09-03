using System;
using QBuild.Utilities;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Camera
{
    public class CameraPresenter : IDisposable, IInitializable
    {
        [Inject]
        public CameraPresenter(CameraModel cameraModel, CircleCamera cameraView, CameraInput cameraInput,
            CameraScriptableObject cameraScriptableObject)
        {
            _cameraModel = cameraModel;
            _cameraView = cameraView;
            _cameraInput = cameraInput;
            _cameraScriptableObject = cameraScriptableObject;
        }

        public void Initialize()
        {
            _cameraInput.OnCameraMove += CameraMove;
            _cameraModel.OnCameraDirectionChanged += CameraTurn;
            _cameraModel.OnLookAtChanged += _cameraView.SetLookAt;

            _cameraModel.SetCameraDirection(Direction.East);

            _cameraScriptableObject.HeightOffset.Subscribe(
                height => _cameraView.SetCameraHeight(height)
            );
            _cameraScriptableObject.Distance.Subscribe(
                distance => _cameraView.SetCameraRadius(distance)
            );

            _cameraScriptableObject.angleOffset.Subscribe(
                angle => _cameraView.SetCameraAngle(angle)
            );
        }

        public void Dispose()
        {
            _cameraInput.OnCameraMove -= CameraMove;
            _cameraModel.OnCameraDirectionChanged -= CameraTurn;
            _cameraModel.OnLookAtChanged -= _cameraView.SetLookAt;
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
        private readonly CameraScriptableObject _cameraScriptableObject;
        private Vector3 _centerPosition;
    }
}