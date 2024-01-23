using UnityEngine;

namespace QBuild.Camera
{
    public class NewCameraWork : MonoBehaviour
    {
        [SerializeField] private Vector2 _mouseInitAxis;
        [SerializeField] private Vector2 _mouseAxis;

        [SerializeField, Tooltip("�}�E�X���x")] private float mouseSensitivity;

        [SerializeField, Tooltip("���삵�����J����")] private GameObject mainCamera;

        [SerializeField, Tooltip("���_�i�v���C���[�j")] private Transform origin;

        [SerializeField, Tooltip("���_����J�����̋���")]
        private float cameraDistance;

        [SerializeField, Tooltip("�J�����̏c�����~�b�g")]
        private float cameraHorizontalLimit;

        [SerializeField] private CameraInputProvider _cameraInputProvider;

        private void Update()
        {
            CameraPositionUpdate();
        }

        private void CameraPositionUpdate()
        {
            if (_cameraInputProvider != null)
            {
                var inputValue = _cameraInputProvider.GetValue();
                _mouseAxis.x += inputValue.x * mouseSensitivity;
                _mouseAxis.y += inputValue.y * -mouseSensitivity;
            }
            else if (Input.GetMouseButton(0))
            {
                _mouseAxis.x += Input.GetAxis("Mouse X") * mouseSensitivity;

                _mouseAxis.y += Input.GetAxis("Mouse Y") * -mouseSensitivity;
            }

            var limit = cameraHorizontalLimit * Mathf.Deg2Rad;
            _mouseAxis.y = Mathf.Clamp(_mouseAxis.y, -limit, limit);

            mainCamera.transform.position =
                new Vector3(
                    Mathf.Sin(_mouseAxis.x) * Mathf.Cos(_mouseAxis.y) * cameraDistance,
                    Mathf.Sin(_mouseAxis.y) * cameraDistance,
                    Mathf.Cos(_mouseAxis.x) * Mathf.Cos(_mouseAxis.y) * cameraDistance
                );

            mainCamera.transform.LookAt(origin);
        }

        public void SetTarget(Transform target)
        {
            origin = target;
            _mouseAxis = _mouseInitAxis;
            CameraPositionUpdate();
        }
    }
}