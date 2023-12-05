using UnityEngine;
using UnityEngine.SceneManagement;

namespace  QBuild.Scene
{
    [CreateAssetMenu(fileName = "newSceneManageSO", menuName = "Data/Manage Data/ScecneManager SO")]
    public class SceneManagerSO : ScriptableObject
    {
        [SerializeField,Tooltip("タイトルシーンの名前")] private string _titleSceneName = "";
        [SerializeField,Tooltip("ステージセレクトシーンの名前")] private string _stageSelectSceneName = "";
        [SerializeField,Tooltip("ゲームシーンの名前")] private string _gameSceneName = "";
        
        public void ChangeScene(SelectScene selectScene)
        {
            string changeSceneName = "";
            switch (selectScene)
            {
                case SelectScene.Title:
                    changeSceneName = _titleSceneName;
                    break;
                case SelectScene.StageSelect:
                    changeSceneName = _stageSelectSceneName;
                    break;
                case SelectScene.Game:
                    changeSceneName = _gameSceneName;
                    break;
            }
            
            
            SceneLoad(changeSceneName);
        }

        private void SceneLoad(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public enum SelectScene
    {
        Title,
        StageSelect,
        Game,
    }
}
