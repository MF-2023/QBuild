using UnityEngine;
using QBuild.Scene;
using UnityEngine.Serialization;

namespace QBuild.GameCycle.Title
{
    public class TitleAction : MonoBehaviour
    {
        [Header("シーン管理")] [SerializeField,Tooltip("ゲームセレクトのシーン番号")] private int _nextSceneIndex = 0;
        //[SerializeField] private SceneManagerSO _sceneManagerSO = null;
        //[FormerlySerializedAs("nextScene")] [SerializeField] private SelectScene _nextScene = SelectScene.StageSelect;
        
        [Header("タイトルシーン管理")]
        [SerializeField] private GameObject _pressPushButtonText = null;
        [SerializeField] private GameObject _buttons = null;
        [SerializeField] private Popup _optionPopup = null;
        [SerializeField] private Popup _gameEndPopup = null;
        [SerializeField] private float _fadeTime = 0.5f;
        
        private void Start()
        {
            ShowPressPushButtonText();
            _optionPopup.gameObject.SetActive(false);
            _gameEndPopup.gameObject.SetActive(false);
        }
        
        public void ShowButtons()
        {
            _pressPushButtonText.SetActive(false);
            _buttons.SetActive(true);
        }
        
        public void ShowPressPushButtonText()
        {
            _pressPushButtonText.SetActive(true);
            _buttons.SetActive(false);
        }
        
        public void ShowOptionPopup()
        {
            _optionPopup.gameObject.SetActive(true);
            _optionPopup.ShowPopup();
        }

        public void ShowGameEndPopup()
        {
            _gameEndPopup.gameObject.SetActive(true);
            _gameEndPopup.ShowPopup();
        }

        public void SceneChangeFadeOut()
        {
            SceneManager.ChangeSceneWait(_fadeTime, _nextSceneIndex);
        }
    }
}
