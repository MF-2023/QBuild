using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace SoVariableTool.ScriptGenerator
{
    public static class ScriptGenerator
    {
        public static void GenerateScript()
        {
            var generatorTypes = TypeCache.GetTypesDerivedFrom<ICodeGeneratable>();

            var changed = false;
            foreach (var t in generatorTypes)
            {
                var generator = (ICodeGeneratable)Activator.CreateInstance(t);
                var context = new GeneratorContext();
                generator.Execute(context);

                if (SaveScriptFromContext(context))
                {
                    changed = true;
                }
            }

            if (!changed) return;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public static void GenerateScript(ICodeGeneratable generatable)
        {
            var changed = false;
            
            var context = new GeneratorContext();
            generatable.Execute(context);

            if (SaveScriptFromContext(context))
            {
                changed = true;
            }

            if (!changed) return;

            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        static bool SaveScriptFromContext(GeneratorContext context)
        {
            var changed = false;

            var preFolderPath = context.PreScriptPath;

            if (!Directory.Exists(preFolderPath))
            {
                Directory.CreateDirectory(preFolderPath);
            }

            foreach (var scriptGenerateInfo in context.ScriptGenerateInfos)
            {
                var directoryName = preFolderPath + "/" + Path.GetDirectoryName(scriptGenerateInfo.ScriptPath);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                var filePath = directoryName + "/" + Path.GetFileName(scriptGenerateInfo.ScriptPath);

                if (File.Exists(filePath))
                {
                    var text = File.ReadAllText(filePath);
                    if (text == scriptGenerateInfo.Code)
                    {
                        continue;
                    }
                }

                File.WriteAllText(filePath, scriptGenerateInfo.Code);
                changed = true;
            }

            return changed;
        }
    }
}