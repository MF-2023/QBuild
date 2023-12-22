using QBuild.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace QBuild.GameCycle
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private SelectStageSO _selectStageSO;

        private void Awake()
        {
            //Addressables.LoadAssetAsync<GameObject>()
            Instantiate(_selectStageSO.SelectStageData._stage);
        }
    }
}
