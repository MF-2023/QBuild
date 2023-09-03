using System;
using QBuild.Const;
using QBuild.Stage;
using QBuild.Utilities;
using UniRx;
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
            StageScriptableObject stageScriptableObject, CameraScriptableObject cameraScriptableObject)
        {
            _cameraModel = cameraModel;
            _cameraView = cameraView;
            _cameraInput = cameraInput;
            _stageScriptableObject = stageScriptableObject;
            _cameraScriptableObject = cameraScriptableObject;
        }

        public void Initialize()
        {
            CreateCenter();

            _cameraInput.OnCameraMove += CameraMove;
            _cameraModel.OnCameraDirectionChanged += CameraTurn;
            _cameraModel.OnLookAtChanged += _cameraView.SetLookAt;

            _cameraModel.SetLookAt(_center.transform);
            _cameraModel.SetCameraDirection(Direction.East);

            _cameraScriptableObject.Height.Subscribe(
                height => _cameraView.SetCameraHeight(height)
            );
            _cameraScriptableObject.Distance.Subscribe(
                distance => _cameraView.SetCameraRadius(distance)
            );

            _cameraScriptableObject.CenterOffset.Subscribe(
                offset => _center.transform.position = _centerPosition + offset
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

        private void CreateCenter()
        {
            _center = new GameObject("Center");
            _centerPosition = new Vector3(_stageScriptableObject.Width / 2, 0, _stageScriptableObject.Depth / 2);
            _centerPosition -= BlockConst.BlockSizeHalf;
            _centerPosition += _cameraScriptableObject.CenterOffset.Value;
            _center.transform.position = _centerPosition;
        }

        private readonly CameraModel _cameraModel;
        private readonly CircleCamera _cameraView;
        private readonly CameraInput _cameraInput;
        private readonly StageScriptableObject _stageScriptableObject;
        private readonly CameraScriptableObject _cameraScriptableObject;
        private GameObject _center;
        private Vector3 _centerPosition;
    }
}