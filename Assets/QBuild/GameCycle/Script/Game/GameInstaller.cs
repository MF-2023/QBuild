using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using QBuild.Scene;
using QBuild.Stage;
using SoVariableTool;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace QBuild.GameCycle
{
    interface ILoadable
    {
        Task Load();
    }

    public class GameInstaller : MonoBehaviour, ILoadable
    {
        [SerializeField] private SelectStageSO _selectStageSO;
        [SerializeField] private GameBuild _gameBuild;
        [SerializeField] private GameObject _stageContainer;
        private AsyncOperationHandle<GameObject> _handler;
        [SerializeField] private AssetReferenceT<UnitScriptableEventObject> _unitScriptableEventObject;
        private AsyncOperationHandle<UnitScriptableEventObject> _unitHandler;
        private async void Awake()
        {
            await LoadStage();

            //Instantiate(_selectStageSO.SelectStageData._stage);
        }

        private async Task LoadStage()
        {

            await UniTask.Yield();
            var stage = Instantiate(_selectStageSO.SelectStageData.GetStagePrefab(), _stageContainer.transform);
            var spawnPoint = FindFirstObjectByType(typeof(PlayerSpawnPoint)) as PlayerSpawnPoint;
            await UniTask.Yield();
            _gameBuild.Bind(spawnPoint, _selectStageSO.SelectStageData.GetQuantitySpawnConfiguratorObject());
            _gameBuild.Build();
        }

        private void OnDestroy()
        {
        }

        public async Task Load()
        {
            await LoadStage();
        }
    }
}