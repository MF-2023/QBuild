using UnityEngine;

namespace QBuild.Stage
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private void Awake()
        {
            if (transform.position.x % 1 != 0 || transform.position.y % 1 != 0 || transform.position.z % 1 != 0)
            {
                {
                    transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y),
                        Mathf.Round(transform.position.z));
                }
            }
        }

        public Vector3 GetSpawnPoint()
        {
            return transform.position + new Vector3(0.5f, 2f, 0.5f);
        }
    }
}