using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace QBuild.StageSelect.Landmark
{
    public class LandmarkInformationBinder : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _stageName;
        [SerializeField] private Image _stageImage;
        [SerializeField] private GameObject[] _itemImages = new GameObject[3];
        [SerializeField] private GameObject[] _difficultyImages = new GameObject[5];
        [SerializeField] private GameObject _isClearedImage;

        public void SetStageName(string stageName)
        {
            _stageName.text = stageName;
        }

        public void SetStageImage(Sprite stageImage)
        {
            _stageImage.sprite = stageImage;
        }

        public void SetItemImages(int itemCount)
        {
            for (int i = 0; i < _itemImages.Length; i++)
            {
                _itemImages[i].SetActive(i < itemCount);
            }
        }

        public void SetDifficultyImages(int difficulty)
        {
            for (int i = 0; i < _difficultyImages.Length; i++)
            {
                _difficultyImages[i].SetActive(i < difficulty);
            }
        }

        public void SetIsClearedImage(bool isCleared)
        {
            _isClearedImage.SetActive(isCleared);
        }
    }
}