using UnityEngine;
using UnityEngine.InputSystem;

namespace QBuild.Control
{
    public class InputManager : MonoBehaviour
    {
        public void OnPlayerMove(InputValue value)
        {
            Debug.Log(value.Get<Vector2>());
        }
    }
}