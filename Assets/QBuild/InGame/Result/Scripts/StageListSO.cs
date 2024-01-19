using System.Collections;
using System.Collections.Generic;
using QBuild.StageEditor;
using UnityEngine;

namespace  QBuild.Stage
{
    [CreateAssetMenu(menuName = "Data/StageList",fileName = ("NewStageList"))]
    public class StageListSO : ScriptableObject
    {
        //ステージの順序を管理するリスト
        public List<StageEditor.StageData> StagegList = new List<StageData>();
    }
}
