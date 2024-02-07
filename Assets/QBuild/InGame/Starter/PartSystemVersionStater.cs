﻿using System;
using QBuild.Camera;
using QBuild.Part;
using QBuild.Part.HolderView;
using QBuild.Part.PartScriptableObject;
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
            
            // Health
            builder.RegisterInstance(_healthBar);
            
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
            builder.RegisterEntryPoint<PlayerHealthPresenter>();
            
            builder.RegisterInstance(_partHolderView);
            builder.RegisterInstance(_partListScriptableObject);
            builder.Register<HolderPresenter>(Lifetime.Singleton);

            builder.RegisterInstance(_partRepository);
            builder.RegisterEntryPoint<PlacePresenter>();
        }

        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CurrentStageVariable _currentStageVariable;
        [SerializeField] private PartHolderView _partHolderView;
        [SerializeField] private BasePartSpawnConfiguratorObject _partListScriptableObject;
        
        [SerializeField] private PartRepository _partRepository;
        
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        
        [SerializeField] private NewCameraWork _cameraView;
    }
}