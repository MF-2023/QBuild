using System.Collections;
using System.Collections.Generic;
using QBuild.StageEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.StageSelect.Landmark
{
    public class LandmarkInformation : MonoBehaviour
    {
        [Header("対象のランドマークのスクリプタブルオブジェクトを設定してください")]
        public StageData _stageData;
    }
}