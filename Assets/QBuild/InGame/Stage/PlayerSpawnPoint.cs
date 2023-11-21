using UnityEngine;

namespace QBuild.Stage
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private void Awake()
        {

        }

        public Vector3 GetSpawnPoint()
        {
            return transform.position + new Vector3(0.5f, 2f, 0.5f);
        }
    }
}