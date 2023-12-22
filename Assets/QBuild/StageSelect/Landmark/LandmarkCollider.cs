using UnityEngine;

namespace QBuild.StageSelect.Landmark
{
    public class LandmarkCollider : MonoBehaviour
    {
        //Reaction Area
        [SerializeField, Header("UIが表示される当たり判定の大きさを設定してください")]
        private float _colliderSize = 2.0f;

        private LandmarkGenerator _landmarkGenerator;

        // Start is called before the first frame update
        void Start()
        {
            var col = this.gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(_colliderSize, _colliderSize, _colliderSize);

            if (TryGetComponent(out LandmarkGenerator component))
                _landmarkGenerator = this.gameObject.GetComponent<LandmarkGenerator>();
            else
                Debug.LogError("LandmarkGeneratorがアタッチされていません", this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            _landmarkGenerator.SetLandmarkEnable();
        }

        private void OnTriggerExit(Collider other)
        {
            _landmarkGenerator.SetLandmarkDisable();
        }

        private void OnTriggerStay(Collider other)
        {
            var pos = StageSelectManager._Instance.GetCameraPosition();
            _landmarkGenerator.SetLandmarkLookAt(pos);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var pos = this.gameObject.transform.position;
            Gizmos.DrawWireCube(pos, new Vector3(_colliderSize, _colliderSize, _colliderSize));
        }
    }
}