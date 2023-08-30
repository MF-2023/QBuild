using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Camera
{
    public class CameraScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CameraPresenter>();
            builder.Register<CameraInput>(Lifetime.Singleton);

            builder.RegisterInstance(_cameraView);
            builder.RegisterInstance(_cameraScriptableObject);
        }
        
        [SerializeField] private CircleCamera _cameraView;
        [SerializeField] private CameraScriptableObject _cameraScriptableObject;
    }
}