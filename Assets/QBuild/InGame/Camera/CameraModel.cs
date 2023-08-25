using System;
using QBuild.Utilities;
using UnityEngine;

namespace QBuild.Camera
{
    public class CameraModel
    {
        public event Action<Direction> OnCameraDirectionChanged;
        public event Action<Transform> OnLookAtChanged; 

        public void TurnLeft()
        {
            _cameraDir = _cameraDir.TurnLeft();
            OnCameraDirectionChanged?.Invoke(_cameraDir);
        }
        
        public void TurnRight()
        {
            _cameraDir = _cameraDir.TurnRight();
            OnCameraDirectionChanged?.Invoke(_cameraDir);
        }
        
        public void SetCameraDirection(Direction direction)
        {
            _cameraDir = direction;
            OnCameraDirectionChanged?.Invoke(_cameraDir);
        }
        
        public void SetLookAt(Transform lookAt)
        {
            _lookAt = lookAt;
            OnLookAtChanged?.Invoke(_lookAt);
        }
        
        public Direction GetCameraDirection()
        {
            return _cameraDir;
        }
        
        private Direction _cameraDir = Direction.East;
        private Transform _lookAt;
    }
}