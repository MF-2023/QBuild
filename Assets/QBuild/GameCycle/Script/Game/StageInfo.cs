using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.GameCycle
{
    public class StageInfo : MonoBehaviour
    {
        [SerializeField] private Transform _initPlayerPosition;

        public Transform InitPlayerPosition { get { return _initPlayerPosition; } }
    }
}