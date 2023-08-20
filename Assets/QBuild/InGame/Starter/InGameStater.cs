using System;
using QBuild.Condition;
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
            builder.Register<MinoDebugFactory>(Lifetime.Singleton).As<IMinoFactory>();
            builder.Register<StageFactory>(Lifetime.Singleton);

            builder.Register<BlockStore>(Lifetime.Singleton);
            builder.Register<MinoStore>(Lifetime.Singleton);

            builder.Register<BlockUseCase>(Lifetime.Singleton);
            builder.Register<MinoUseCase>(Lifetime.Singleton);
            builder.Register<StabilityCalculator>(Lifetime.Singleton);

            builder.RegisterEntryPoint<BlockPresenter>(Lifetime.Singleton);

            builder.RegisterInstance(_blockManager);
            builder.RegisterInstance(_currentStageVariable);
            builder.RegisterInstance(_currentStageVariable.RuntimeValue);
            builder.RegisterInstance(_planeBlockType);
            builder.RegisterInstance(_blockPrefabInfo);
            builder.RegisterInstance(_faceJointMatrix);
        }

        [SerializeField] private BlockManager _blockManager;
        [SerializeField] private CurrentStageVariable _currentStageVariable;
        [SerializeField] private BlockType _planeBlockType;
        [SerializeField] private BlockCreateInfo _blockPrefabInfo;
        [SerializeField] private FaceJointMatrix _faceJointMatrix;
    }
}