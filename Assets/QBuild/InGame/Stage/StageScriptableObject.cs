using QBuild.Const;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.Stage
{
    /// <summary>
    /// 大きさなどステージの構成を定義するScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "StageLevel", menuName = EditorConst.VariablePrePath + "StageLevel",
        order = EditorConst.OtherOrder)]
    public class StageScriptableObject : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _depth;
        [SerializeField] private int _height;
        
        [SerializeField] private Vector3Int _playerSpawnPosition;
        [SerializeField] private Vector3Int _MinoSpawnPosition;
        
        public int Width => _width;
        public int Depth => _depth;
        public int Height => _height;
        
        public Vector3Int Size => new(_width, _height, _depth);
        
        public Vector3Int PlayerSpawnPosition => _playerSpawnPosition;
        public Vector3Int MinoSpawnPosition => _MinoSpawnPosition;
    }
}