using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Stage
{
    [CreateAssetMenu(fileName = "newStageData", menuName = "Data/Stage Data")]
    public class SelectStageSO : ScriptableObject
    {
        [SerializeField,Tooltip("ステージが選択されていない時に渡されるデータ")] private StageSO _initStageData;
        private StageSO _selectStageData;

        public StageSO GetStageData
        {
            get
            {
                if (_selectStageData == null) return _initStageData;
                return _selectStageData;
            }
        }
    }
}
