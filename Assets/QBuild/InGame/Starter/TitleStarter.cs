using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Starter
{
    public class TitleStarter : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("TitleStarter.Configure");
            builder.Register<@InputSystem>(Lifetime.Singleton);

        }

    }
}