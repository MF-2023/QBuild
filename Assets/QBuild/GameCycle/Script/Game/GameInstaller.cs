using System.Collections;
using QBuild.Stage;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace QBuild.GameCycle
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private SelectStageSO _selectStageSO;

        private void Awake()
        {
            LoadStage();
            
            //Instantiate(_selectStageSO.SelectStageData._stage);
        }

        private void LoadStage()
        {
            _selectStageSO.SelectStageData.GetReferenceStagePrefab()
                                          .LoadAssetAsync<GameObject>()
                                          .Completed += handler =>
                                            {
                                                Instantiate((handler.Result));
                                            };
        }
    }
}
