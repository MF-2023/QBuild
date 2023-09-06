using QBuild.Camera;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.DebugSystem
{
    public class DebugSystemScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_stageTemplateGenerator, Lifetime.Singleton);
        }
        
        [SerializeField] private StageTemplateGenerator _stageTemplateGenerator;
    }
}