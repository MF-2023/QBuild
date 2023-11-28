using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SoVariableTool.ScriptableVariable
{
    public class VariablePostprocessor : AssetPostprocessor
    {
        private static readonly HashSet<string> GuidsCache = new();
        
        private static string InitializedSessionKey => $"VariablePostprocessor_IsInitialized";
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var isInitialized = SessionState.GetBool(InitializedSessionKey, false);
            if (!isInitialized)
            {
                RegenerateAllGuids();
                SessionState.SetBool(InitializedSessionKey, true);
            }
            else
            {
                OnAssetCreated(importedAssets);
                OnAssetDeleted(deletedAssets);
                OnAssetMoved(movedFromAssetPaths, movedAssets);
            }
        }
        private static List<T> FindAll<T>(string path = "") where T : ScriptableObject
        {
            var scripts = new List<T>();
            var searchFilter = $"t:{typeof(T).Name}";
            var soNames = path == ""
                ? AssetDatabase.FindAssets(searchFilter)
                : AssetDatabase.FindAssets(searchFilter, new[] { path });

            foreach (var soName in soNames)
            {
                var soPath = AssetDatabase.GUIDToAssetPath(soName);
                var script = AssetDatabase.LoadAssetAtPath<T>(soPath);
                if (script == null)
                    continue;

                scripts.Add(script);
            }

            return scripts;
        }
        private static void RegenerateAllGuids()
        {
            var scriptableVariableBases = FindAll<ScriptableVariableObjectBase>();
            foreach (var scriptableVariable in scriptableVariableBases)
            {
                scriptableVariable.Guid = GenerateGuid(scriptableVariable);
                GuidsCache.Add(scriptableVariable.Guid);
            }
        }

        private static void OnAssetCreated(string[] importedAssets)
        {
            foreach (var assetPath in importedAssets)
            {
                if (GuidsCache.Contains(assetPath))
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<ScriptableVariableObjectBase>(assetPath);
                if (asset == null)
                    continue;

                asset.Guid = GenerateGuid(asset);
                GuidsCache.Add(asset.Guid);
            }
        }

        private static void OnAssetDeleted(string[] deletedAssets)
        {
            foreach (var assetPath in deletedAssets)
            {
                if (!GuidsCache.Contains(assetPath))
                    continue;

                GuidsCache.Remove(assetPath);
            }
        }

        private static void OnAssetMoved(string[] movedFromAssetPaths, string[] movedAssets)
        {
            OnAssetDeleted(movedFromAssetPaths);
            OnAssetCreated(movedAssets);
        }
        
        private static string GenerateGuid(ScriptableObject scriptableObject)
        {
            var path = AssetDatabase.GetAssetPath(scriptableObject);
            var guid = AssetDatabase.AssetPathToGUID(path);
            return guid;
        }
    }
}