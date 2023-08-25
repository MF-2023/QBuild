using System;
using QBuild.Camera;
using QBuild.Utilities;
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
        public MinoInput(@InputSystem inputSystem, CameraModel cameraModel)
        {
            inputSystem.Enable();
            _inputSystem = inputSystem;
            _inputSystem.InGame.BlockMove.performed += MinoMove;
            _cameraModel = cameraModel;
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
            var direction = _cameraModel.GetCameraDirection();

            //directionを元にmoveを回転させる
            switch (direction)
            {
                case Direction.East:
                    break;
                case Direction.South:
                    move = new Vector3Int(-move.z, 0, move.x);
                    break;
                case Direction.West:
                    move = new Vector3Int(-move.x, 0, -move.z);
                    break;
                case Direction.North:
                    move = new Vector3Int(move.z, 0, -move.x);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _onMinoMove.OnNext(move);
        }

        private readonly Subject<Vector3Int> _onMinoMove = new();

        private readonly InputSystem _inputSystem;
        
        private readonly CameraModel _cameraModel;
    }
}