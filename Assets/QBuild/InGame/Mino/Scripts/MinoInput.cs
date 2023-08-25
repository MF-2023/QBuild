using System;
using QBuild.Camera;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace QBuild.Mino
{
    public class MinoInput : IDisposable
    {
        public IObservable<Vector3Int> OnMinoMove => _onMinoMove;

        [Inject]
        public MinoInput(@InputSystem inputSystem, CameraModel cameraInput)
        {
            inputSystem.Enable();
            _inputSystem = inputSystem;
            _inputSystem.InGame.BlockMove.performed += MinoMove;
        }

        public void Dispose()
        {
            _inputSystem.InGame.BlockMove.performed -= MinoMove;
        }


        private void MinoMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
            Debug.Log($"InputValue:{inputValue}");

            var move = new Vector3Int((int)inputValue.x, 0, (int)inputValue.y);
            _onMinoMove.OnNext(move);
        }

        private readonly Subject<Vector3Int> _onMinoMove = new();

        private readonly InputSystem _inputSystem;
        
        
    }
}