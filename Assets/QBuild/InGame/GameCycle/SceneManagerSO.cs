using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace  QBuild.Scene
{
    public class SceneManagerSO : ScriptableObject
    {
        [SerializeField,Tooltip("�^�C�g���V�[���̖��O")] private string _titleSceneName = "";
        [SerializeField,Tooltip("�X�e�[�W�Z���N�g�V�[���̖��O")] private string _stageSelectSceneName = "";
        [SerializeField,Tooltip("�Q�[���V�[���̖��O")] private string _gameSceneName = "";

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
