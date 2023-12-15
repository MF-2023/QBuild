using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Stage
{
    [CreateAssetMenu(fileName = "newStageData", menuName = "Data/Stage Data")]
    public class SelectStageSO : ScriptableObject
    {
        [SerializeField,Tooltip("�X�e�[�W���I������Ă��Ȃ����ɓn�����f�[�^")] private StageSO _initStageData;
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
