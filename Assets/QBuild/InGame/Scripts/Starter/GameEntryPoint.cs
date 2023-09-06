using System;
using QBuild.Camera;
using QBuild.Camera.Center;
using QBuild.DebugSystem;
using QBuild.Starter;
using UnityEngine;
using VContainer.Unity;

namespace QBuild.Scripts.Starter
{
    public class GameEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            _inGameStater.Build();

            using (LifetimeScope.EnqueueParent(_inGameStater))
            {
                var scope = _inGameStater.CreateChildFromPrefab(_cameraScope);
                scope.Build();
                using (LifetimeScope.EnqueueParent(scope))
                {
                    var centerScope = scope.CreateChildFromPrefab(_centerScope);
                    centerScope.Build();
                }
                
                var debugScope = _inGameStater.CreateChildFromPrefab(_debugSystemScope);
                debugScope.Build();
            }
        }

        [SerializeField] private InGameStater _inGameStater;
        [SerializeField] private CameraScope _cameraScope;
        [SerializeField] private CenterScope _centerScope;
        [SerializeField] private DebugSystemScope _debugSystemScope;
    }
}