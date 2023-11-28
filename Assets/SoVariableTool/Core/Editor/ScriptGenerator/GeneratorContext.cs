using System.Collections.Generic;
using UnityEngine.Serialization;

namespace SoVariableTool.ScriptGenerator
{
    public struct ScriptGenerateInfo
    {
        [FormerlySerializedAs("ScriptName")] public string ScriptPath;
        public string Code;
    }
    public class GeneratorContext
    {
        private readonly List<ScriptGenerateInfo> _scriptGenerateInfos = new();
        public IEnumerable<ScriptGenerateInfo> ScriptGenerateInfos => _scriptGenerateInfos;
        
        public void AddScriptGenerateInfo(ScriptGenerateInfo scriptGenerateInfo)
        {
            _scriptGenerateInfos.Add(scriptGenerateInfo);
        }
        public string PreScriptPath;
    }
}