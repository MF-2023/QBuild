using System;
using QBuild.Stage;
using UniRx;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace QBuild.Camera.Center
{
    public class CenterPresenter : IDisposable, IInitializable
    {
        private readonly CameraModel _cameraModel;
        private readonly CameraScriptableObject _cameraScriptableObject;
        private readonly CenterView _centerView;

        [Inject]
        public CenterPresenter(CameraModel cameraModel, StageScriptableObject stageScriptableObject,
            CameraScriptableObject cameraScriptableObject, CenterView centerView)
        {
            _cameraModel = cameraModel;
            _cameraScriptableObject = cameraScriptableObject;
            _centerView = centerView;
        }

        public void Dispose()
        {
            Object.Destroy(_centerView);
        }

        public void Initialize()
        {
            _cameraModel.SetLookAt(_centerView.transform);
            _cameraScriptableObject.CenterOffset.Subscribe(
                offset => _centerView.transform.position = _centerView.GetCenterPosition() + offset
            );
        }
    }
}