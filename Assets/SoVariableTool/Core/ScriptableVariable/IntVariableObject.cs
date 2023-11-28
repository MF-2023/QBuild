using System;
using UnityEngine;

namespace SoVariableTool.ScriptableVariable
{
    [CreateAssetMenu(fileName = "IntVariable", menuName = ConstParameter.VariablePrePath + "Int")]
    public class IntVariableObject : ScriptableVariable<int>
    {
        void test()
        {
        }
    }
}