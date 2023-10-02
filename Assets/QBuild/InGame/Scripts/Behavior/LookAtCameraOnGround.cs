using UnityEngine;

namespace QBuild.Behavior
{
    /// <summary>
    /// Y軸を基準に90度回転させて後にカメラの方向を向く
    /// </summary>
    public class LookAtCameraOnGround : MonoBehaviour
    {
        private void Start()
        {
            _cameraTransform = UnityEngine.Camera.main.transform;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }

        private void Update()
        {
            var cameraDirectionOnGround =
                Vector3.Scale(
                    new Vector3(_cameraTransform.position.x, transform.position.y, _cameraTransform.position.z) -
                    transform.position, new Vector3(1, 0, 0));
            cameraDirectionOnGround.Normalize();

            var targetRotation = Quaternion.LookRotation(cameraDirectionOnGround) * Quaternion.Euler(90f, 0f, 0f);
            transform.rotation = targetRotation;
        }

        private Transform _cameraTransform;
    }
}