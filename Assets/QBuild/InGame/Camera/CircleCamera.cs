using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace QBuild.Camera
{
    /// <summary>
    /// カメラを円形に配置し、ブレンド遷移を行うクラス
    /// </summary>
    public class CircleCamera : CinemachineVirtualCameraBase
    {
        public override void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
        {
            var best = _virtualCameraBase[_currentCameraIndex];
            if (best == null) return;

            var sita = 360f / _virtualCameraBase.Count;
            var position = _lookAt != null ? _lookAt.position : new Vector3(0, 0, 0);
            position.y = transform.position.y;
            var bestTransform = best.transform;
            bestTransform.position = position + Quaternion.Euler(0, sita * _currentCameraIndex + _offsetRadian, 0) *
                Vector3.forward * _radius;
            if (_prevCameraIndex == -1)
            {
                _prevCameraIndex = _currentCameraIndex;
            }

            if (_currentCameraIndex != _prevCameraIndex)
            {
                var blend =
                    new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseOut, 1);
                _activeBlend = CreateBlend(_virtualCameraBase[_prevCameraIndex], best, blend, _activeBlend);
                CinemachineCore.Instance.GenerateCameraActivationEvent(best, _virtualCameraBase[_prevCameraIndex]);

                _prevCameraIndex = _currentCameraIndex;
            }

            if (_activeBlend != null)
            {
                _activeBlend.TimeInBlend += (deltaTime >= 0) ? deltaTime : _activeBlend.Duration;

                if (_activeBlend.IsComplete)
                    _activeBlend = null;
            }

            if (_activeBlend != null)
            {
                _activeBlend.UpdateCameraState(worldUp, deltaTime);
                _state = _activeBlend.State;
            }
            else if (_prevCameraIndex != -1)
            {
                _state = best.State;
            }

            InvokePostPipelineStageCallback(this, CinemachineCore.Stage.Finalize, ref _state, deltaTime);

        }

        public override Transform Follow
        {
            get => ResolveFollow(follow);
            set => follow = value;
        }

        public override CameraState State => _state;

        public override Transform LookAt
        {
            get => ResolveLookAt(_lookAt);
            set => _lookAt = value;
        }

        [NoSaveDuringPlay] [VcamTargetProperty]
        public Transform follow;

        public void SetCameraIndex(int index)
        {
            _currentCameraIndex = index;
        }

        public void SetLookAt(Transform lookAt)
        {
            _lookAt = lookAt;
        }
        
        public void SetCameraHeight(float height)
        {
            Debug.Log("SetCameraHeight");
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
        
        public void SetCameraRadius(float radius)
        {
            _radius = radius;
        }
        
        public void SetCameraAngle(float angle)
        {
            _offsetRadian = angle;
        }
        
        public int CurrentCameraIndex => _currentCameraIndex;
        
        private CameraState _state = CameraState.Default;
        [SerializeField] private Transform _lookAt;
        [SerializeField] private int _currentCameraIndex = 0;
        private int _prevCameraIndex = -1;
        [SerializeField] private List<CinemachineVirtualCameraBase> _virtualCameraBase;
        [SerializeField] private float _radius = 15f;
        [SerializeField] private float _offsetRadian = 0f;
        private CinemachineBlend _activeBlend = null;
    }
}