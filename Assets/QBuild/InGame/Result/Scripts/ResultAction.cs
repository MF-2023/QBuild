using QBuild.Scene;
using SoVariableTool;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultAction : MonoBehaviour
    {
        [SerializeField] private UnitScriptableEventObject _onClickNextStageEvent;
        public void OnClickRetry()
        {
            SceneManager.ChangeSceneWait(SceneBuildIndex.Game, SceneChangeEffect.Fade, 0.1f);
        }
        
        public void OnClickStageSelect()
        {
            SceneManager.ChangeSceneWait(SceneBuildIndex.StageSelect, SceneChangeEffect.Fade, 0.5f);
        }
        
        public void OnClickTitle()
        {
            SceneManager.ChangeSceneWait(SceneBuildIndex.Title, SceneChangeEffect.Fade, 0.5f);
        }
        
        public void OnClickNext()
        {
            _onClickNextStageEvent.Raise();
        }
    }
}