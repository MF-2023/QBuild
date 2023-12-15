using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Stage
{
    [CreateAssetMenu(fileName = "newStageData", menuName = "Data/Stage Data")]
    public class StageSO : ScriptableObject
    {
        [SerializeField] private GameObject _stagePrefab;
        [SerializeField] private GameObject _playerPrefab;

        public GameObject StagePrefab { get { return _stagePrefab; } }
        public GameObject PlayerPrefab { get { return _playerPrefab; } }
    }
}
