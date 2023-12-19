using QBuild.StageEditor;
using UnityEngine;
using QBuild.StageSelect.Landmark;

/*
 * データを保存
 * 
 * var saveData = new LandmarkInformationModel()
 * {
 *  isClear = false,
 *  getCrystalNum = 1,
 * };
 * SaveDataController.SetSaveDataFromLandmark(landmarkInformationScriptable,saveData);
 */

/*
 * データを取得
 *
 * var saveData = SaveDataController.GetSaveDataFromLandmark(landmarkInformationScriptable);
 * Debug.Log("isClear:" + saveData.isClear + " getCrystalNum:" + saveData.getCrystalNum);
 */

namespace QBuild.PlayerPrefsController
{
    public static class SaveDataController
    {
        public static LandmarkInformationModel GetSaveDataFromLandmark(StageData stageData)
        {
            var key = stageData._fileName;
            var data = GetSaveDataKey(key);
            return data;
        }

        public static LandmarkInformationModel GetSaveDataKey(string key)
        {
            var json = PlayerPrefs.GetString(key);

            if (string.IsNullOrEmpty(json))
                return new LandmarkInformationModel();

            return JsonUtility.FromJson<LandmarkInformationModel>(json);
        }

        public static void SetSaveDataFromLandmark(StageData stageData,
            LandmarkInformationModel saveData)
        {
            var key = stageData._fileName;
            SetSaveDataFromKey(key, saveData);
        }

        public static void SetSaveDataFromKey(string key, LandmarkInformationModel saveData)
        {
            var json = JsonUtility.ToJson(saveData);

            PlayerPrefs.SetString(key, json);
        }

        public static void DeleteSaveData(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static bool HasSaveData(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}