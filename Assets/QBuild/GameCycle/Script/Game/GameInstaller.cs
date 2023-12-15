using QBuild.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.GameCycle
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private SelectStageSO _selectStageSO;

        private void Awake()
        {
            StageSO stageSO = _selectStageSO.GetStageData;
            Instantiate(stageSO.StagePrefab).TryGetComponent<StageInfo>(out StageInfo stageInfo);
            GameObject player = Instantiate(stageSO.PlayerPrefab);
            player.transform.position = stageInfo.InitPlayerPosition.position;
            player.transform.rotation = stageInfo.InitPlayerPosition.rotation;
        }
    }
}
