using QBuild.Mino;
using QBuild.Stage;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace QBuild.Starter
{
    /// <summary>
    /// ゲーム開始するための依存解決を行うクラス
    /// </summary>
    public class InGameStater : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("InGameStater.Configure");
            builder.Register<BlockFactory>(Lifetime.Singleton);
            builder.Register<MinoFactory>(Lifetime.Singleton);
            
            builder.Register<StageFactory>(Lifetime.Singleton);

            builder.RegisterInstance(_blockManager);
            builder.RegisterInstance(_currentStageVariable);
            builder.RegisterInstance(_currentStageVariable.RuntimeValue);
            builder.RegisterInstance(_planeBlockType);
            builder.RegisterInstance(_blockPrefab);
            //builder.RegisterEntryPoint<>()
        }

        
        [SerializeField] private BlockManager _blockManager;
        [SerializeField] private CurrentStageVariable _currentStageVariable;
        [FormerlySerializedAs("_planeBlockGenerator")] [SerializeField] private BlockType _planeBlockType;
        [SerializeField] private GameObject _blockPrefab;
        
    }
}