using SherbetInspector.Core.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.Player
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeReference] private PlayerRespawnProvider _playerRespawnProvider;
        
        private void Update()
        {
            if (transform.position.y < -5.0f)
            {
                Respawn();
            }
        }

        [Button("リスポーン")]
        private void Respawn()
        {
            transform.position = _playerRespawnProvider.RespawnPosition;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}