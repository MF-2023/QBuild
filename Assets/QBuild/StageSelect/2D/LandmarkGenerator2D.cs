using QBuild.PlayerPrefsController;
using QBuild.StageSelect.Landmark;
using UnityEngine;
using UnityEngine.UI;

namespace QBuild.StageSelect._2D
{
    public class LandmarkGenerator2D : MonoBehaviour
    {
        [SerializeField] private StageDataScriptableEventObject _onStartButtonClicked;
        private LandmarkInformationBinder _landmarkInformationBinder;

        private Selectable _startButton;
        private void Start()
        {
            if (!TryGetComponent(out LandmarkInformation landmarkInformation))
            {
                Debug.LogError("LandmarkInformationがアタッチされていません", this.gameObject);
                return;
            }
            
            if (!TryGetComponent(out _landmarkInformationBinder))
            {
                Debug.LogError("LandmarkInformationBinderがアタッチされていません", this.gameObject);
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
                _landmarkInformationBinder.SetStageName(scriptableObject.GetStageName());
                _landmarkInformationBinder.SetStageImage(scriptableObject.GetStageImage());
                _landmarkInformationBinder.SetDifficultyImages(scriptableObject.GetStageDifficult());
            }

            //Load SaveData
            {
                var landmarkInformationScriptable = landmarkInformation._stageData;
                var saveData = SaveDataController.GetSaveDataFromLandmark(landmarkInformationScriptable);
                _landmarkInformationBinder.SetItemImages(saveData._getCrystalNum);
                _landmarkInformationBinder.SetIsClearedImage(saveData._isClear);
            }

            {
                Debug.Log($"LandmarkGenerator:{landmarkInformation._stageData.name}", this);
                var scriptableObject = landmarkInformation._stageData;
                _landmarkInformationBinder.BindStartButton(() =>
                {
                    Debug.Log($"StartButtonClicked:{scriptableObject.name}", _landmarkInformationBinder.StartButton);
                    _onStartButtonClicked.Raise(new object[]{
                        scriptableObject
                    });
                });
                _startButton = _landmarkInformationBinder.StartButton;
            }
        }
    }
}