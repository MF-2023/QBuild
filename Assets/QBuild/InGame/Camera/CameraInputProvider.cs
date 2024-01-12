using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QBuild.Camera
{
    public class CameraInputProvider : MonoBehaviour
    {
        [SerializeField] private InputActionReference _cameraMove;

        private void Start()
        {
            _cameraMove.action.Enable();
        }

        public Vector2 GetValue()
        {
            return _cameraMove.action.ReadValue<Vector2>();
        }
    }
}