using QBuild.Part;
using UnityEngine;

namespace QBuild.Player
{
    public class PlayerRespawnOnPartProvider : PlayerRespawnProvider
    {
        private Vector3 _respawnPosition;

        public override Vector3 RespawnPosition => _respawnPosition;

        public void SetPart(PartView partView)
        {
            SetRespawnPosition(partView.GetSpawnPosition());
        }

        public void SetRespawnPosition(Vector3 position)
        {
            _respawnPosition = position;
        }
    }
}