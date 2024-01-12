using System;
using UnityEngine.InputSystem;
using VContainer;

namespace QBuild.Camera
{
    public class CameraInput : IDisposable
    {
        public event Action<int> OnCameraMove;
        
        
        [Inject]
        public CameraInput(@InputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _inputSystem.Enable();
            //_inputSystem.InGame.CameraMove.performed += CameraMove;
        }

        public void Dispose()
        {
            //_inputSystem.InGame.CameraMove.performed -= CameraMove;
        }
        
        private void CameraMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<float>();
            OnCameraMove?.Invoke((int)inputValue);
        }

        private readonly @InputSystem _inputSystem;
    }
}