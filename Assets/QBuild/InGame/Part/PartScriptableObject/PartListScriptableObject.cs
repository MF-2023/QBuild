using System.Collections.Generic;
using QBuild.Const;
using UnityEngine;

namespace QBuild.Part
{
    /// <summary>
    /// ゲームで利用するパーツのリスト
    /// </summary>
    [CreateAssetMenu(fileName = "PartList", menuName = EditorConst.VariablePrePath + "PartList",
        order = EditorConst.OtherOrder)]
    public class PartListScriptableObject : ScriptableObject
    {
        [SerializeField] private List<BlockPartScriptableObject> _parts;

        public IEnumerable<BlockPartScriptableObject> GetParts()
        {
            return _parts;
        }
    }
}