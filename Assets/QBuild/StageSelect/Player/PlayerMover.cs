using UnityEngine;
using UnityEngine.InputSystem;

namespace QBuild.StageSelect.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        private Vector2 _moveDirection;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            var direction = new Vector3(_moveDirection.x, 0, _moveDirection.y) * (Time.deltaTime * _moveSpeed);
            transform.position += direction;

            if (direction.magnitude > 0)
            {
                var lookRotation = Quaternion.LookRotation(direction);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotateSpeed);
            }
            
            StageSelectManager._Instance.SetCameraPosition(transform.position);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }
    }
}