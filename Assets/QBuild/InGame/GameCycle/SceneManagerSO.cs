using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace  QBuild.Scene
{
    public class SceneManagerSO : ScriptableObject
    {
        [SerializeField,Tooltip("タイトルシーンの名前")] private string _titleSceneName = "";
        [SerializeField,Tooltip("ステージセレクトシーンの名前")] private string _stageSelectSceneName = "";
        [SerializeField,Tooltip("ゲームシーンの名前")] private string _gameSceneName = "";

        public void ChangeScene(SelectScene selectScene)
        {
            switch (selectScene)
            {
                case SelectScene.Title:
                    SceneManager.LoadScene(_titleSceneName);
                    break;
                case SelectScene.StageSelect:
                    SceneManager.LoadScene(_stageSelectSceneName);
                    break;
                case SelectScene.Game:
                    SceneManager.LoadScene(_gameSceneName);
                    break;
            }
        }
    }

    public enum SelectScene
    {
        Title,
        StageSelect,
        Game,
    }
}
