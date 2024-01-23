using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QBuild.PlayerPrefsController;
using QBuild.StageEditor;
using SoVariableTool;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QBuild.StageSelect.Landmark
{
    public class LandmarkGenerator : MonoBehaviour
    {
        [SerializeField] private float _landmarkImageHeight = 2.0f;

        [SerializeField] private GameObject _landmarkImagePrefab;
        private GameObject _landmarkImage;

        [SerializeField] private float _popUpPower = 0.05f;
        [SerializeField] private float _popUpTime = 0.5f;
        private Sequence _popUpTween;
        
        [SerializeField] private StageDataScriptableEventObject _onStartButtonClicked;

        private Selectable _startButton;
        // Start is called before the first frame update
        void Start()
        {
            var tran = transform;
            var pos = tran.position;
            pos.y += _landmarkImageHeight;
            _landmarkImage = Instantiate(_landmarkImagePrefab, pos, Quaternion.identity, tran);
            _landmarkImage.SetActive(false);
            FadeAnimations(0.0f, 0.0f);

            if (!TryGetComponent(out LandmarkInformation landmarkInformation))
            {
                Debug.LogError("LandmarkInformationがアタッチされていません", this.gameObject);
                return;
            }

            if (!_landmarkImage.TryGetComponent(out LandmarkInformationBinder landmarkInformationBinder))
            {
                Debug.LogError("LandmarkInformationBinderがアタッチされていません", _landmarkImage);
                return;
            }

            //Set SaveData Sample
            /*
            {
                var landmarkInformationScriptable = landmarkInformation._landmarkInformationScriptable;
                var saveData = new LandmarkInformationModel
                {
                    _isClear = true,
                    _getCrystalNum = 2,
                };
                SaveDataController.SetSaveDataFromLandmark(landmarkInformationScriptable, saveData);
            }
            */

            //Set LandmarkInformation
            {
                var scriptableObject = landmarkInformation._stageData;
                landmarkInformationBinder.SetStageName(scriptableObject.GetStageName());
                landmarkInformationBinder.SetStageImage(scriptableObject.GetStageImage());
                landmarkInformationBinder.SetDifficultyImages(scriptableObject.GetStageDifficult());
            }

            //Load SaveData
            {
                var landmarkInformationScriptable = landmarkInformation._stageData;
                var saveData = SaveDataController.GetSaveDataFromLandmark(landmarkInformationScriptable);
                landmarkInformationBinder.SetItemImages(saveData._getCrystalNum);
                landmarkInformationBinder.SetIsClearedImage(saveData._isClear);
            }

            {
                Debug.Log($"LandmarkGenerator:{landmarkInformation._stageData.name}", this);
                var scriptableObject = landmarkInformation._stageData;
                landmarkInformationBinder.BindStartButton(() =>
                {
                    Debug.Log($"StartButtonClicked:{scriptableObject.name}", landmarkInformationBinder.StartButton);
                    _onStartButtonClicked.Raise(new object[]{
                        scriptableObject
                    });
                });
                _startButton = landmarkInformationBinder.StartButton;
            }
        }

        public void SetLandmarkEnable()
        {
            _popUpTween?.Complete();
            _popUpTween = DOTween.Sequence();
//            EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
            _startButton.Select();
            Debug.Log($"SetLandmarkEnable:{_startButton.gameObject.name}",this);
            _landmarkImage.SetActive(true);
            _popUpTween.Append(_landmarkImage.transform.DOPunchScale(new Vector3(_popUpPower, _popUpPower, _popUpPower),
                _popUpTime));
            _popUpTween.Join(DOVirtual.DelayedCall(0, () => { FadeAnimations(1.0f, _popUpTime); }));
        }

        public void SetLandmarkDisable()
        {
            _popUpTween?.Complete();
            _popUpTween = DOTween.Sequence();
            EventSystem.current.SetSelectedGameObject(null);
            Debug.Log($"SetLandmarkDisable:{_startButton.gameObject.name}",this);
            _popUpTween.Append(
                _landmarkImage.transform.DOPunchScale(new Vector3(-_popUpPower, -_popUpPower, -_popUpPower),
                    _popUpTime));
            _popUpTween.Join(DOVirtual.DelayedCall(0, () => { FadeAnimations(0.0f, _popUpTime); }));
            _popUpTween.OnComplete(() => { _landmarkImage.SetActive(false); });
        }

        private void FadeAnimations(float fadeValue, float animationTime)
        {
            var images = _landmarkImage.GetComponentsInChildren<Image>();
            var textMeshPros = _landmarkImage.GetComponentsInChildren<TextMeshProUGUI>();
            var sprites = _landmarkImage.GetComponentsInChildren<SpriteRenderer>();

            foreach (var image in images)
                image.DOFade(fadeValue, animationTime);
            foreach (var textMeshPro in textMeshPros)
                textMeshPro.DOFade(fadeValue, animationTime);
            foreach (var sprite in sprites)
                sprite.DOFade(fadeValue, animationTime);
        }

        public void SetLandmarkLookAt(Vector3 position)
        {
            _landmarkImage.transform.LookAt(position);
            _landmarkImage.transform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
}