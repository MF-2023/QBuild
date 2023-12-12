using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.StageEditor
{
    [CreateAssetMenu(menuName = "Tools/QBuild/StageEditor/StageData")]
    public class StageData : ScriptableObject
    {
        public GameObject stage;
        public string fileName;
        public string stageName;
        public int crystalCount;
        public Texture stageImage;

        public StageData Clone()
        {
            var clone = CreateInstance<StageData>();
            clone.stage = stage;
            clone.fileName = fileName;
            clone.stageName = stageName;
            clone.crystalCount = crystalCount;
            clone.stageImage = stageImage;
            return clone;
        }
    }

}
