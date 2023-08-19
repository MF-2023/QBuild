using QBuild.Const;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.Stage
{
    [CreateAssetMenu(fileName = "StageLevel", menuName = EditorConst.VariablePrePath + "StageLevel",
        order = EditorConst.OtherOrder)]
    public class StageScriptableObject : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _depth;
        [SerializeField] private int _height;
        
        public int Width => _width;
        public int Depth => _depth;
        public int Height => _height;
    }
}