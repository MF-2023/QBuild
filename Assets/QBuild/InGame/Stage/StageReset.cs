using QBuild.Part;
using QBuild.Player.Controller;
using UnityEngine;
using VContainer;

namespace QBuild.Stage
{
    public class StageReset : MonoBehaviour
    {
        [SerializeField] private PartRepository _partRepository;
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        private PlayerController _playerController = null;
        
        public void OnReset()
        {
            if (_playerController == null)
            {
                _playerController = FindObjectOfType<PlayerController>();
            }
            _partRepository.AllDestroy();
            _playerController.transform.position = _playerSpawnPoint.GetSpawnPoint();
            _playerController.GetComponent<PartPlacer>().OnReset();
        }
    }
}