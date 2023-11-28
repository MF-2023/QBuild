using System;
using UnityEngine;

namespace SoVariableTool.ScriptableVariable
{
    [Serializable]
    public struct VariableSave<T>
    {
        public T SaveValue;
    }

    public static class VariableUtility
    {
        public static void Save<T>(ScriptableVariable<T> variable)
        {
            VariableSave<T> save = new()
            {
                SaveValue = variable.Value
            };
            PlayerPrefs.SetString(variable.Guid, JsonUtility.ToJson(save));
        }

        public static T Load<T>(ScriptableVariable<T> variable)
        {
            if (PlayerPrefs.HasKey(variable.Guid) == false)
                return default;

            var json = PlayerPrefs.GetString(variable.Guid);
            if (string.IsNullOrEmpty(json))
                return default;

            var save = JsonUtility.FromJson<VariableSave<T>>(json);
            return save.SaveValue;
        }
    }
}