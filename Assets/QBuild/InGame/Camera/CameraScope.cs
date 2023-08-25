using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Camera
{
    public class CameraScope : LifetimeScope
    {
        [SerializeField] private CircleCamera _cameraView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CameraPresenter>();
            builder.Register<CameraInput>(Lifetime.Singleton);

            builder.RegisterInstance(_cameraView);
        }
    }
}