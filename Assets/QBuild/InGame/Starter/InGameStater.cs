﻿using System;
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
            builder.Register<StageFactory>(Lifetime.Singleton);

            builder.Register<BlockStore>(Lifetime.Singleton);

            builder.Register<BlockService>(Lifetime.Singleton);
            builder.Register<StabilityCalculator>(Lifetime.Singleton);

            builder.Register<MinoDebugFactory>(Lifetime.Singleton).As<IMinoFactory>();
            builder.Register<MinoStore>(Lifetime.Singleton);
            builder.Register<MinoService>(Lifetime.Singleton);
            builder.Register<FallMino>(Lifetime.Singleton);
            builder.Register<MinoInput>(Lifetime.Singleton);
            builder.Register<MinoPhysicsSimulation>(Lifetime.Singleton);
            builder.RegisterEntryPoint<MinoPresenter>();
            
            builder.RegisterEntryPoint<MinoEventLifeCycle>();
            builder.Register<MinoFallTick>(Lifetime.Singleton);
            builder.Register<MinoDestroyTick>(Lifetime.Singleton);
            builder.Register<MinoSpawnTick>(Lifetime.Singleton);

            builder.RegisterEntryPoint<BlockPresenter>(Lifetime.Singleton);
            
            builder.Register<@InputSystem>(Lifetime.Singleton);

            builder.RegisterInstance(_blockManager);
            builder.RegisterInstance(_currentStageVariable);
            builder.RegisterInstance(_currentStageVariable.RuntimeValue);
            builder.RegisterInstance(_planeBlockType);
            builder.RegisterInstance(_blockPrefabInfo);
            builder.RegisterInstance(_faceJointMatrix);
            builder.RegisterInstance(_minoTypeList);

        }

        [SerializeField] private BlockManager _blockManager;
        [SerializeField] private CurrentStageVariable _currentStageVariable;
        [SerializeField] private BlockType _planeBlockType;
        [SerializeField] private BlockCreateInfo _blockPrefabInfo;
        [SerializeField] private FaceJointMatrix _faceJointMatrix;
        [SerializeField] private MinoTypeList _minoTypeList;
    }
}