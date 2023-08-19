using System;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Stage
{
    /// <summary>
    /// 現在のステージを保持する変数
    /// </summary>
    [CreateAssetMenu(fileName = "CurrentStageVariable", menuName = EditorConst.VariablePrePath + "CurrentStage",
        order = EditorConst.OtherOrder)]
    public class CurrentStageVariable : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private StageScriptableObject _initialValue;

        [NonSerialized] public StageScriptableObject RuntimeValue;

        public void OnAfterDeserialize()
        {
            RuntimeValue = _initialValue;
        }

        public void OnBeforeSerialize()
        {
        }
    }
}