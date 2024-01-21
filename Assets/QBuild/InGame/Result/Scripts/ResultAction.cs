using QBuild.Scene;
using UnityEngine;

namespace QBuild.Result
{
    public class ResultAction : MonoBehaviour
    {
        public void OnClickRetry()
        {
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
        }
    }
}