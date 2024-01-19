using System;
using QBuild.Camera;
using QBuild.Camera.Center;
using QBuild.Starter;
using UnityEngine;
using VContainer.Unity;

namespace QBuild.Starter
{
    public class PartSystemVersionEntryPoint : MonoBehaviour
    {
        private void Awake()
        {
            _inGameStater.Build();

            using (LifetimeScope.EnqueueParent(_inGameStater))
            {
            }
        }

        [SerializeField] private PartSystemVersionStater _inGameStater;
        [SerializeField] private CameraScope _cameraScope;
        [SerializeField] private CenterScope _centerScope;
    }
}