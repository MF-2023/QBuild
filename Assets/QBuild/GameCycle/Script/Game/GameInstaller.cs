using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using QBuild.Scene;
using QBuild.Stage;
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
    
    public class GameInstaller : MonoBehaviour,ILoadable
    {
        [SerializeField] private SelectStageSO _selectStageSO;
        private AsyncOperationHandle<GameObject> _handler;

        private async void Awake()
        {
            await LoadStage();
            
            //Instantiate(_selectStageSO.SelectStageData._stage);
        }

        private async Task LoadStage()
        {
            _handler = _selectStageSO.SelectStageData.GetStagePrefab()
                .LoadAssetAsync<GameObject>();

            await _handler.Task;
            
            Instantiate(_handler.Result);
        }
        
        private void OnDestroy()
        {
            Addressables.Release(_handler);
        }

        public async Task Load()
        {
             await LoadStage();
        }
    }
}
