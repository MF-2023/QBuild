using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace QBuild.StageEditor
{
    /// <summary>
    /// ステージに関するデータ
    /// </summary>
    [CreateAssetMenu(menuName = "Tools/QBuild/StageEditor/StageData")]
    public class StageData : ScriptableObject
    {
        public GameObject _stage;
        [SerializeField] private bool _isExistWarningItem;
        public bool IsExistWarningItem() => _isExistWarningItem;

        [SerializeField] private AssetReferenceGameObject _stagePrefab;
        public AssetReferenceGameObject GetStagePrefab() => _stagePrefab;

        [SerializeField] private string _fileName;
        public string GetFileName() => _fileName;


        [SerializeField] private string _stageName;
        public string GetStageName() => _stageName;

        [SerializeField] private int _stageDifficult;
        public int GetStageDifficult() => _stageDifficult;

        [SerializeField] private int _crystalCount;
        public int GetCrystalCount() => _crystalCount;

        [SerializeField] private Texture2D _stageImage;
        public Texture2D GetStageImage() => _stageImage;

        [SerializeField] private Vector3Int _stageArea;
        public Vector3Int GetStageArea() => _stageArea;
    }
}