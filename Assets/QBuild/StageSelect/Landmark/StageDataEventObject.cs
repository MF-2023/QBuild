using System;
using QBuild.StageEditor;
using SoVariableTool;
using UnityEngine;
using UnityEngine.Events;

namespace QBuild.StageSelect.Landmark
{
    [CreateAssetMenu(fileName = "event_StageData", menuName = "SoVariableTool/ScriptableEvents/StageData")]
    public class StageDataScriptableEventObject : ScriptableEventObject<StageData, StageDataUnityEvent>
    {
    }
    
    [Serializable]
    public class StageDataUnityEvent : UnityEvent<StageData>, IDynamicEventUseable {}

}