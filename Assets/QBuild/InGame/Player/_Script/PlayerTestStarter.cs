
using VContainer;
using VContainer.Unity;

namespace QBuild.Player
{
    /// <summary>
    /// Player�̃e�X�g�p�̃X�^�[�^�[
    /// </summary>
    public class PlayerTestStater : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<@InputSystem>(Lifetime.Singleton);
        }
    }
}