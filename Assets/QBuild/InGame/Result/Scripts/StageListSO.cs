using System.Collections;
using System.Collections.Generic;
using QBuild.StageEditor;
using UnityEngine;

namespace  QBuild.Stage
{
    [CreateAssetMenu(menuName = "Data/StageList",fileName = ("NewStageList"))]
    public class StageListSO : ScriptableObject
    {
        //�X�e�[�W�̏������Ǘ����郊�X�g
        public List<StageEditor.StageData> StagegList = new List<StageData>();
    }
}
