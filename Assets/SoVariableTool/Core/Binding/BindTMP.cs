using SoVariableTool.ScriptableVariable;
using TMPro;
using UnityEngine;

namespace SoVariableTool.Binding
{
    public class BindTMP : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text = null;

        [SerializeField] private ScriptableVariableObjectBase _variable;
        private void Awake()
        {
            _text.text = _variable.GetValue().ToString();
            
        }
    }
}