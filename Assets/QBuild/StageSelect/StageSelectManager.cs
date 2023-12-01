using UnityEngine;

namespace QBuild.StageSelect
{
    public class StageSelectManager : MonoBehaviour
    {
        public static StageSelectManager _Instance;
        [SerializeField] private GameObject _cameraOffsetObject, _camera;
        [SerializeField] private float _cameraMoveSpeed = 5.0f;

        private void Awake()
        {
            if (_Instance == null)
                _Instance = this;
            else
                Destroy(this);
        }

        public void SetCameraPosition(Vector3 position)
        {
            var pos = Vector3.Lerp(_cameraOffsetObject.transform.position, position,
                Time.deltaTime * _cameraMoveSpeed);
            _cameraOffsetObject.transform.position = pos;
        }

        public Vector3 GetCameraPosition()
        {
            return _camera.transform.position;
        }
    }
}