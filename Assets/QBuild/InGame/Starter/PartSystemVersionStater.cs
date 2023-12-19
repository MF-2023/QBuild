using System;
using QBuild.Camera;
using QBuild.Condition;
using QBuild.Mino;
using QBuild.Mino.ProvisionalMino;
using QBuild.Part;
using QBuild.Part.HolderView;
using QBuild.Part.Presenter;
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
                var playerController = container.Instantiate(_playerPrefab, _playerSpawnPoint.GetSpawnPoint(),
                    Quaternion.identity, null);
                var playerTransform = playerController.transform;
                playerTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                _cameraView.SetTarget(playerTransform);
                return playerController;
            }, Lifetime.Singleton);
            builder.RegisterEntryPoint<PlayerPresenter>();
            
            builder.RegisterInstance(_partHolderView);
            builder.RegisterInstance(_partListScriptableObject);
            builder.Register<HolderPresenter>(Lifetime.Singleton);

            builder.RegisterInstance(_partRepository);
            builder.RegisterEntryPoint<PlacePresenter>();
        }

        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private CurrentStageVariable _currentStageVariable;
        [SerializeField] private PartHolderView _partHolderView;
        [SerializeField] private PartListScriptableObject _partListScriptableObject;
        
        [SerializeField] private PartRepository _partRepository;
        
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        
        [SerializeField] private NewCameraWork _cameraView;
    }
}