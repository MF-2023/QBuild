﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

namespace SoVariableTool.ScriptableEvent
{
    internal static class EventTypeScriptGenerator
    {
        [MenuItem("Tools/SoVariable/GenerateBuiltInTypeEvent", priority = 1)]
        private static void GenerateBuiltInType()
        {
            Type[] builtInTypes =
            {
                typeof(int), typeof(float), typeof(double), typeof(string), typeof(bool)
            };
            GenerateTypes("EventBuiltInTypes.cs", builtInTypes);
        }
        
        [MenuItem("Tools/SoVariable/GenerateCustomTypeEvent", priority = 11)]
        private static void GenerateCustomType()
        {
            Type[] builtInTypes =
            {
                typeof(Unit)
            };
            GenerateTypes("CustomTypes.cs", builtInTypes);
        }
        
        private static void GenerateTypes(string packageName,IEnumerable<Type> types)
        {

            var builder = new StringBuilder();
            foreach (var type in types)
            {
                builder.AppendLine(GenerateEventClass(type));
                Generate($"EventObject/{type.Name}ScriptableEventObject.cs",
                    GenerateDecorate(GenerateScriptableEventObjectClass(type), true, false));
            }

            Generate($"{packageName}", GenerateDecorate(builder.ToString(), false, true));
        }
        private static void Generate(string filepath, string code)
        {
            var folderPath = PreFolderPath;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            var hierarchy = filepath.Split('/');
            var path = folderPath;
            for (var i = 0; i < hierarchy.Length; i++)
            {
                path += "/" + hierarchy[i];
                if (i == hierarchy.Length - 1) break;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                if (text == code)
                {
                    return;
                }
            }

            File.WriteAllText(path, code);


            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        private static string GenerateDecorate(string code, bool isUnityEngine, bool isUnityEngineEvent)
        {
            var builder = new StringBuilder();
            builder.AppendLine("// <auto-generated/>");
            builder.AppendLine("using System;");
            if (isUnityEngineEvent) builder.AppendLine("using UnityEngine.Events;");
            if (isUnityEngine) builder.AppendLine("using UnityEngine;");
            builder.AppendLine();
            builder.AppendLine("namespace SoVariableTool");
            builder.AppendLine("{");
            builder.AppendLine(code);
            builder.AppendLine("}");

            return builder.ToString();
        }

        private static string GenerateEventClass(Type type)
        {
            var code =
                $@"
    // <auto-generated/>
    [Serializable]
    public class {type.Name}UnityEvent : UnityEvent<{type.FullName}>, IDynamicEventUseable {{}}
";
            return code;
        }

        private static string GenerateScriptableEventObjectClass(Type type)
        {
            var code =
                $@"
    // <auto-generated/>
    [CreateAssetMenu(fileName = ""event_{type.Name}"", menuName = ""SoVariableTool/ScriptableEvents/{type.Name}"")]
    public class {type.Name}ScriptableEventObject : ScriptableEventObject<{type.Name}, {type.Name}UnityEvent>
    {{
    }}
";
            return code;
        }


        private static readonly string PreFolderPath = "Assets/SoVariableTool/Core/ScriptableEvent/";
    }
}