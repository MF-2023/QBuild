using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace QBuild.StageSelect.Landmark
{
    public class LandmarkInformationBinder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _stageName;
        
        [SerializeField] private Image _stageImage;
        
        [SerializeField] private Image[] _itemImages = new Image[3];
        [SerializeField] private Sprite _collectedItemImage;
        
        [SerializeField] private Image[] _difficultyImages = new Image[5];
        [SerializeField] private Sprite _enabledDifficultyImage;
        
        [SerializeField] private Image _isClearedImage;

        private void Start()
        {
            if ( _stageName == null )
                Debug.LogError("StageImageが設定されていません",this);
            if ( _stageImage == null )
                Debug.LogError("StageImageが設定されていません",this);
            if ( _itemImages == null )
                Debug.LogError("ItemImagesが設定されていません",this);
            if (_collectedItemImage == null)
                Debug.LogError("CollectedItemImageが設定されていません", this);
            if ( _difficultyImages == null )
                Debug.LogError("DifficultyImagesが設定されていません",this);
            if ( _enabledDifficultyImage == null )
                Debug.LogError("EnabledDifficultyImageが設定されていません",this);
            if ( _isClearedImage == null )
                Debug.LogError("IsClearedImageが設定されていません",this);
        }

        public void SetStageName(string stageName)
        {
            if ( _stageName == null ) return;
            _stageName.text = stageName;
        }

        public void SetStageImage(Sprite stageImage)
        {
            if ( _stageImage == null ) return;
            _stageImage.sprite = stageImage;
        }

        public void SetItemImages(int itemCount)
        {
            if (_collectedItemImage == null) return;
            for (int i = 0; i < _itemImages.Length; i++)
            {
                //_itemImages[i].gameObject.SetActive(i < itemCount);
                if (i < itemCount)
                {
                    _itemImages[i].sprite = _collectedItemImage;
                }
            }
        }

        public void SetDifficultyImages(int difficulty)
        {
            if ( _enabledDifficultyImage == null ) return;
            for (int i = 0; i < _difficultyImages.Length; i++)
            {
                if (i < difficulty)
                {
                    _difficultyImages[i].sprite = _enabledDifficultyImage;
                }
                else
                {
                    _difficultyImages[i].gameObject.SetActive(i < difficulty);
                }
            }
        }

        public void SetIsClearedImage(bool isCleared)
        {
            if ( _isClearedImage == null ) return;
            _isClearedImage.gameObject.SetActive(isCleared);
        }
    }
}