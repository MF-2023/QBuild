using System.Text;
using SoVariableTool.ScriptGenerator;

namespace SoVariableTool.ScriptableVariable
{
    public class VariableScriptGenerator : ICodeGeneratable
    {
        public void Execute(GeneratorContext context)
        {
            StringBuilder code = new();
            code.AppendLine("using System;");
        }
    }
}