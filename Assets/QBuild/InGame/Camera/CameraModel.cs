using System;
using UnityEngine;

namespace QBuild.Camera
{
    public class CameraModel
    {
        public event Action<int> OnCameraDirectionChanged;
        public event Action<Transform> OnLookAtChanged; 

        public void TurnLeft()
        {
            _cameraIndex--;
            if(_cameraIndex < 0) _cameraIndex = 3;
            
            OnCameraDirectionChanged?.Invoke(_cameraIndex);
        }
        
        public void TurnRight()
        {
            _cameraIndex++;
            if(_cameraIndex > 3) _cameraIndex = 0;

            OnCameraDirectionChanged?.Invoke(_cameraIndex);
        }
        
        public void SetLookAt(Transform lookAt)
        {
            _lookAt = lookAt;
            OnLookAtChanged?.Invoke(_lookAt);
        }
        
        private int _cameraIndex = 0;
        private Transform _lookAt;
    }
}