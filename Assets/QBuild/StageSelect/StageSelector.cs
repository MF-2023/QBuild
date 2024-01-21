using QBuild.Scene;
using QBuild.Stage;
using QBuild.StageEditor;
using UnityEngine;

namespace QBuild.StageSelect
{
    public class StageSelector : MonoBehaviour
    {
        [SerializeField] private float _fadeTime = 0.5f;
        [SerializeField] private SelectStageSO _selectStageSO;
        public void Select(StageData stageData)
        {
            _selectStageSO.SelectStageData = stageData;
            SceneManager.ChangeSceneWait(SceneBuildIndex.Game, SceneChangeEffect.Fade, _fadeTime);
        }
    }
}