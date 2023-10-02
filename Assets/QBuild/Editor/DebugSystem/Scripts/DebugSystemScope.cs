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
        }

        [SerializeField] private StageTemplateGenerator _stageTemplateGenerator;
    }
}