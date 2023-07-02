using System.Collections;
using System.Linq;
using System.Reflection;
using SherbetInspector.Core.Attributes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SherbetInspector.Editor
{
    public static class SherbetEditorGUI
    {
        
        private static readonly GUIStyle ButtonStyle = new GUIStyle(GUI.skin.button) { richText = true };
        
        public static void Button(UnityEngine.Object target, MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().All(p => p.IsOptional))
            {
                ButtonAttribute buttonAttribute =
                    (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                string displayName = string.IsNullOrEmpty(buttonAttribute.DisplayName)
                    ? ObjectNames.NicifyVariableName(methodInfo.Name)
                    : buttonAttribute.DisplayName;

                var enabled = true;
                UnityEditor.EditorGUI.BeginDisabledGroup(!enabled);
                if (GUILayout.Button(displayName, ButtonStyle))
                {
                    object[] defaultParams = methodInfo.GetParameters().Select(p => p.DefaultValue).ToArray();
                    IEnumerator methodResult = methodInfo.Invoke(target,defaultParams) as IEnumerator;
                    if (!Application.isPlaying)
                    {
                        // Set target object and scene dirty to serialize changes to disk
                        EditorUtility.SetDirty(target);

                        var stage = PrefabStageUtility.GetCurrentPrefabStage();
                        EditorSceneManager.MarkSceneDirty(stage != null
                            ? stage.scene
                            : SceneManager.GetActiveScene());
                    }else if (methodResult != null && target is MonoBehaviour behaviour)
                    {
                        behaviour.StartCoroutine(methodResult);
                    }
                }
                EditorGUI.EndDisabledGroup();
                
            }
            else
            {
                var warning = nameof(ButtonAttribute) + " works only on methods with no parameters";
                Debug.LogWarning(warning, context: target);
            }
        }
    }
}