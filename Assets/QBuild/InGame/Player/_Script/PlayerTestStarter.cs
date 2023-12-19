
using VContainer;
using VContainer.Unity;

namespace QBuild.Player
{
    /// <summary>
    /// Playerのテスト用のスターター
    /// </summary>
    public class PlayerTestStater : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<@InputSystem>(Lifetime.Singleton);
        }
    }
}