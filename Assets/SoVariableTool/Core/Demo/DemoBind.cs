// @DemoBind.cs
// @brief
// @author ICE
// @date 2023/10/27
// 
// @details

using UnityEngine;

namespace SoVariableTool.Demo
{
    public class DemoBind : MonoBehaviour
    {
        [SerializeField] private string _demoVariable;

        public string DemoVariable
        {
            get => _demoVariable;
            set => _demoVariable = value;
        }


        private void Update()
        {
            if (_demoVariable != "0") Debug.Log(_demoVariable);
        }
    }
}