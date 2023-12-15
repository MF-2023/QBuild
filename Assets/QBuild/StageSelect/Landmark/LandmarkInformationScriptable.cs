using UnityEngine;

namespace QBuild.StageSelect.Landmark
{
    [CreateAssetMenu(menuName = "Tools/QBuild/StageSelect/LandmarkInformationScriptable")]
    public class LandmarkInformationScriptable : ScriptableObject
    {
        public string _stageName;
        public Sprite _stageImage;

        [field: Range(1, 5)] public int _stageDifficult;
    }
}