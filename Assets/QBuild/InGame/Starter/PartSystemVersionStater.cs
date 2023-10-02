using System;
using QBuild.Camera;
using QBuild.Condition;
using QBuild.Mino;
using QBuild.Mino.ProvisionalMino;
using QBuild.Part;
using QBuild.Part.HolderView;
using QBuild.Player;
using QBuild.Stage;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;
using QBuild.Player.Controller;

namespace QBuild.Starter
{
    /// <summary>
    /// ゲーム開始するための依存解決を行うクラス
    /// </summary>
    public class PartSystemVersionStater : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Input
            builder.Register<@InputSystem>(Lifetime.Singleton);

            // Stage
            builder.RegisterInstance(_currentStageVariable.RuntimeValue);

            // Camera
            builder.Register<CameraModel>(Lifetime.Singleton);
            
            // Player
            builder.Register(container =>
            {
                var playerController = container.Instantiate(_playerPrefab,new Vector3(0.5f,2f,0.5f),Quaternion.identity,null);
                return playerController;
            }, Lifetime.Singleton);
            builder.RegisterEntryPoint<PlayerPresenter>();

            
            builder.Register<NextPartHolder>(Lifetime.Singleton);
            builder.RegisterInstance(_partHolderView);
            builder.RegisterInstance(_partListScriptableObject);
            builder.RegisterEntryPoint<HolderPresenter>();
        }
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private CurrentStageVariable _currentStageVariable;
        [SerializeField] private PartHolderView _partHolderView;
        [SerializeField] private PartListScriptableObject _partListScriptableObject;
        

    }
}