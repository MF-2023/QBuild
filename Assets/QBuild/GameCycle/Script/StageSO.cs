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
    }
}
