using System.Collections;
using System.Collections.Generic;
using QBuild.StageEditor;
using UnityEngine;

namespace QBuild.Stage
{
    [CreateAssetMenu(fileName = "newStageData", menuName = "Data/Stage Data")]
    public class SelectStageSO : ScriptableObject
    {
        private StageData _selectStageData;
        public StageData SelectStageData
        {
            get { return _selectStageData; }
        }
    }
}
