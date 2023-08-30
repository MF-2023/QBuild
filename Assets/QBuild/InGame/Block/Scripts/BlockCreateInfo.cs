using QBuild.Const;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild
{
    /// <summary>
    /// ブロックの生成に必要な情報を持つ
    /// </summary>
    [CreateAssetMenu(fileName = "BlockCreateInfo", menuName = EditorConst.ScriptablePrePath + "BlockCreateInfo", order = EditorConst.OtherOrder)]
    public class BlockCreateInfo : ScriptableObject
    {
        public GameObject Prefab => _prefab;
        [SerializeField] private GameObject _prefab;
    }
}