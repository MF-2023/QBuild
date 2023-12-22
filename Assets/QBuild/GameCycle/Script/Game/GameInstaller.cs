using QBuild.Stage;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QBuild.GameCycle
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private SelectStageSO _selectStageSO;

        private void Awake()
        {
            /*
            _selectStageSO.SelectStageData.GetReferenceStagePrefab().LoadAssetAsync().Completed += handle =>
            {
                Instantiate(handle.Result);
            };
            */
            
            Instantiate(_selectStageSO.SelectStageData._stage);
        }
    }
}
