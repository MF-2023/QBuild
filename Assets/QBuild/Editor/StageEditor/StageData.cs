using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace QBuild.StageEditor
{
    [CreateAssetMenu(menuName = "Tools/QBuild/StageEditor/StageData")]
    public class StageData : ScriptableObject
    {
        public GameObject _stage;
        public string _fileName;
        public string _stageName;
        public int _stageDifficult;
        public int _crystalCount;
        public Texture2D _stageImage;
        public Vector3Int _stageArea;
    }

}
