using System;
using QBuild.Camera;
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
                _inGameStater.CreateChildFromPrefab(_cameraScope);
            }
        }

        [SerializeField] private InGameStater _inGameStater;
        [SerializeField] private CameraScope _cameraScope;
    }
}