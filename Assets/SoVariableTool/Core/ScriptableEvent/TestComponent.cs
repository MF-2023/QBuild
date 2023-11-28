using System;
using UnityEngine;

namespace SoVariableTool
{
    public class TestComponent : MonoBehaviour
    {
        [SerializeField] Int32ScriptableEventObject _int32ScriptableEventObject = null;
        
        public void IntTest(int k)
        {
            Debug.Log(k);
        }
        
        public void BoolTest(bool k)
        {
            Debug.Log(k);
        }

        public void Start()
        {
            _int32ScriptableEventObject.Raise(10);
        }
    }
}