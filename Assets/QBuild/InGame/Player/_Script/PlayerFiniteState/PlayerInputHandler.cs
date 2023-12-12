using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using InputSystem = QBuild.InputSystem;

namespace QBuild.Player.Controller
{
    public class PlayerInputHandler : MonoBehaviour, IDisposable
    {
        public float xInput { get; private set; }
        public float zInput { get; private set; }

        public @InputSystem inputSystem;

        [Inject]
        public void Construct(InputSystem input)
        {
            inputSystem = input;
            inputSystem.Enable();
            inputSystem.InGame.PlayerMove.performed += InputMove;
            inputSystem.InGame.PlayerMove.canceled += InputMove;

        }
        public void Dispose()
        {
            inputSystem.InGame.PlayerMove.performed -= InputMove;
            inputSystem.InGame.PlayerMove.canceled -= InputMove;
        }
        private void InputMove(InputAction.CallbackContext context)
        {
            var inputValue = context.ReadValue<Vector2>();
            xInput = inputValue.x;
            zInput = inputValue.y;
        }
    }
}
