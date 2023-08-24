// @CircleCamera.cs
// @brief
// @author ICE
// @date 2023/08/23
// 
// @details

using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace QBuild.Camera
{
    public class CircleCamera : CinemachineVirtualCameraBase
    {
        public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
        {
            var best = _virtualCameraBase[_currentCameraIndex];
            if (best == null) return;
            _state = best.State;
            
            var sita = 360f / _virtualCameraBase.Count;
            var position = _lookAt.position;
            position.y = transform.position.y;
            var bestTransform = best.transform;
            bestTransform.position = position + Quaternion.Euler(0, sita * _currentCameraIndex + _offsetRadian, 0) * Vector3.forward * _radius;
            
            InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Finalize, ref _state, deltaTime);
        }

        public override Transform Follow
        {
            get => ResolveFollow(follow);
            set => follow = value;
        }

        public override CameraState State
        {
            get => _state;
        }

        public override Transform LookAt
        {
            get => ResolveLookAt(_lookAt);
            set => _lookAt = value;
        }

        [NoSaveDuringPlay] [VcamTargetProperty]
        public Transform follow;

        private CameraState _state = CameraState.Default;
        [SerializeField] private Transform _lookAt;
        [SerializeField] private int _currentCameraIndex = 0;
        [SerializeField] private List<CinemachineVirtualCameraBase> _virtualCameraBase;
        [SerializeField] private float _radius = 15f;
        [SerializeField] private float _offsetRadian = 0f;
    }
}