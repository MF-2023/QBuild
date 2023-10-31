using System;
using QBuild.Player.Controller;
using UnityEngine;
using UnityEngine.InputSystem;

namespace QBuild.Stage.Grid
{
    public class GridViewControl : MonoBehaviour
    {
        private InputSystem _inputSystem;
        [SerializeField] private GameObject _gridView;

        private PlayerController _playerController;
        [SerializeField] private Vector3 _offset;

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerController.OnChangeGridPosition += ChangeGridPosition;
        }

        private void Update()
        {
            if (Keyboard.current.numpad1Key.wasPressedThisFrame)
            {
                _gridView.SetActive(!_gridView.activeSelf);
            }

            //_gridView.transform.position = Convert2(_playerController.transform.position) + _offset;
        }

        private void ChangeGridPosition(Vector3 position)
        {
            position = Convert1(position);
            _gridView.transform.position = position + _offset;
        }

        private Vector3 Convert1(Vector3 position)
        {
            return new Vector3(Mathf.Ceil(position.x), Mathf.Ceil(position.y), Mathf.Ceil(position.z));
        }

        private Vector3 Convert2(Vector3 position)
        {
            return new Vector3(0, Mathf.RoundToInt(position.y), 0);
        }
    }
}