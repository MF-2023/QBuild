using QBuild.Const;
using UnityEngine;

namespace QBuild.Mino.ProvisionalMino
{
    [CreateAssetMenu(fileName = "ProvisionalSetting", menuName = EditorConst.ScriptablePrePath + "ProvisionalSetting",
        order = EditorConst.OtherOrder)]
    public class ProvisionalMinoSetting : ScriptableObject
    {
        public GameObject ProvisionalMinoPrefab => _provisionalMinoPrefab;
        [SerializeField] private GameObject _provisionalMinoPrefab;
    }
}