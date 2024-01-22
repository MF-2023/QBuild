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
            Build();
        }
        
        public void Build()
        {
            _inGameStater.Build();
        }

        [SerializeField] private PartSystemVersionStater _inGameStater;
    }
}