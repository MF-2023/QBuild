using Cinemachine;
using QBuild.Camera;
using QBuild.Part;
using QBuild.Part.HolderView;
using QBuild.Part.PartScriptableObject;
using QBuild.Part.Presenter;
using QBuild.Player;
using QBuild.Player.Controller;
using QBuild.Stage;
using QBuild.Stage.Grid;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace QBuild.GameCycle
{
    public class GameBuild : LifetimeScope
    {
        public void Bind(PlayerSpawnPoint spawnPoint,BasePartSpawnConfiguratorObject partListScriptableObject)
        {
            _playerSpawnPoint = spawnPoint;
            _partListScriptableObject = partListScriptableObject;
        }
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Input
            builder.RegisterInstance(_playerInputActions.InputSystem);

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
                _gridViewControl.Bind();
                _camera.Follow = playerTransform;
                _camera.LookAt = playerTransform;
                return playerController;
            }, Lifetime.Singleton);
            
            builder.RegisterEntryPoint<PlayerPresenter>();

            builder.RegisterInstance(_healthBar);
            builder.RegisterEntryPoint<PlayerHealthPresenter>();
            
            builder.RegisterInstance(_partHolderView);
            builder.RegisterInstance(_partListScriptableObject);
            builder.Register<HolderPresenter>(Lifetime.Singleton);

            builder.RegisterInstance(_partRepository);
            builder.RegisterEntryPoint<PlacePresenter>();
        }
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private InputController _playerInputActions;
        [SerializeField] private PlayerController _playerPrefab;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private CurrentStageVariable _currentStageVariable;
        [SerializeField] private PartHolderView _partHolderView;
        [SerializeField] private BasePartSpawnConfiguratorObject _partListScriptableObject;

        [SerializeField] private PartRepository _partRepository;

        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;

        [SerializeField] private NewCameraWork _cameraView;
        [SerializeField] private GridViewControl _gridViewControl;
    }
}