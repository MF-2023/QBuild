using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  QBuild.Player
{
    [CreateAssetMenu(fileName = "newPlayerProgressData", menuName = "Data/Player Data/Progress Data")]
    public class PlayerCurrentData : ScriptableObject
    {
        [HideInInspector] public bool EndGoalAnimation;
        [HideInInspector] public int CurrentHelth;
    }
}
