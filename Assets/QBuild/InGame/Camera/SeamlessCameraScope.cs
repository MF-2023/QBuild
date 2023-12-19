using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Camera
{
    public class SeamlessCameraScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cameraView);
            builder.RegisterInstance(_cameraScriptableObject);
        }
        
        [SerializeField] private NewCameraWork _cameraView;
        [SerializeField] private CameraScriptableObject _cameraScriptableObject;
    }
}