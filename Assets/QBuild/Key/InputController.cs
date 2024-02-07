using UnityEngine;
using UnityEngine.InputSystem;

namespace QBuild
{
    public class InputController : MonoBehaviour
    {
        private @InputSystem _inputSystem;

        private InputActionMap m_CurrentActionMap;
        public @InputSystem InputSystem => _inputSystem;

        
        private void Awake()
        {
            _inputSystem = new @InputSystem();
            m_CurrentActionMap = _inputSystem.InGame;
            m_CurrentActionMap.Enable();
        }

        public void SetUIActionMap()
        {
            m_CurrentActionMap?.Disable();

            m_CurrentActionMap = _inputSystem.UI;
            m_CurrentActionMap.Enable();
        }

        public void SetInGameActionMap()
        {
            m_CurrentActionMap?.Disable();

            m_CurrentActionMap = _inputSystem.InGame;
            m_CurrentActionMap.Enable();
        }
    }
}