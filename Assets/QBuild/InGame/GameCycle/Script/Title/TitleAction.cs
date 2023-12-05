using UnityEngine;
using QBuild.Scene;
using UnityEngine.Serialization;

namespace QBuild.GameCycle.Title
{
    public class TitleAction : MonoBehaviour
    {
        [Header("シーン管理")]
        [SerializeField] private SceneManagerSO _sceneManagerSO = null;
        [FormerlySerializedAs("nextScene")] [SerializeField] private SelectScene _nextScene = SelectScene.StageSelect;
        
        [Header("タイトルシーン管理")]
        [SerializeField] private GameObject _pressPushButtonText = null;
        [SerializeField] private GameObject _buttons = null;
        [SerializeField] private Popup _optionPopup = null;
        [SerializeField] private FadeInOut _fadeInOut = null;
        
        private void Start()
        {
            ShowPressPushButtonText();
            _optionPopup.gameObject.SetActive(false);
            FadeIn();
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

        public void FadeIn()
        {
            _fadeInOut.gameObject.SetActive(true);
            _fadeInOut.FadeIn(() => _fadeInOut.gameObject.SetActive(false));
        }

        public void FadeOut()
        {
            _fadeInOut.gameObject.SetActive(true);
            _fadeInOut.FadeOut();
        }

        public void SceneChangeFadeOut()
        {
            _fadeInOut.gameObject.SetActive(true);
            _fadeInOut.FadeOut(() => _sceneManagerSO.ChangeScene(_nextScene));
        }
    }
}
