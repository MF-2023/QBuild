using UnityEngine;
using System;

namespace  QBuild.Player
{
    [CreateAssetMenu(fileName = "newPlayerProgressData", menuName = "Data/Player Data/Progress Data")]
    public class PlayerCurrentData : ScriptableObject
    {
        [HideInInspector] public bool EndGoalAnimation;
        [HideInInspector] public bool EndFailedAnimation;
        [HideInInspector] public int CurrentHelth;

        public Action ChangeGoalState;
    }
}
