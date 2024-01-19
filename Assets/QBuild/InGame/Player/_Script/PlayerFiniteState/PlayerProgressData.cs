using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  QBuild.Player
{
    [CreateAssetMenu(fileName = "newPlayerProgressData", menuName = "Data/Player Data/Progress Data")]
    public class PlayerProgressData : ScriptableObject
    {
        [HideInInspector] public bool EndGoalAnimation;
    }
}
