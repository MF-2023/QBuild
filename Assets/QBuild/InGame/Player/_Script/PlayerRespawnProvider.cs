using UnityEngine;

namespace QBuild.Player
{
    public abstract class PlayerRespawnProvider : MonoBehaviour
    {
        public abstract Vector3 RespawnPosition { get;}
    }
}