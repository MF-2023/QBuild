using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Camera.Center
{
    public class CenterScope: LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<CenterPresenter>();
            builder.RegisterComponentOnNewGameObject<CenterView>(Lifetime.Scoped, "Center");
        }
        
    }
}