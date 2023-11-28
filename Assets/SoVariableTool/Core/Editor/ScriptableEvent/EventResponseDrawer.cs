using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace SoVariableTool.ScriptableEvent
{
    [CustomPropertyDrawer(typeof(EventResponse))]
    public class EventResponseDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            
            var eventResponseInspector = new VisualElement();
            
            var scriptableEventObject = new PropertyField(property.FindPropertyRelative("_scriptableEvent"));
            // ScriptableObjectが差し替えられた際にイベントを実行する
            scriptableEventObject.TrackPropertyValue(property.FindPropertyRelative("_scriptableEvent"),
                (prop) => OnChangedScriptableEvent(property, prop)
            );
            //OnChangedScriptableEvent(property, property.FindPropertyRelative("_scriptableEvent"));
            eventResponseInspector.Add(scriptableEventObject);
            
            var response = new PropertyField(property.FindPropertyRelative("_response"));
            
            eventResponseInspector.Add(new Label("Response"));
            eventResponseInspector.Add(response);
            
            return eventResponseInspector;
        }
        
        private static void OnChangedScriptableEvent(SerializedProperty viewEventResponses,SerializedProperty scriptableEventProperty)
        {
            var scriptableEvent =
                scriptableEventProperty.objectReferenceValue as
                    ScriptableEventObjectBase;
            if (scriptableEvent == null) return;

            var responseProperty = viewEventResponses.FindPropertyRelative("_response");
            var unityEventBaseProperty = responseProperty.FindPropertyRelative("_unityEventBase");

            var newUnityEvent = scriptableEvent.CreateUnityEvent();
            
            var unityEvent = unityEventBaseProperty.managedReferenceValue as UnityEventBase;
            if (unityEvent == null)
            {
                unityEventBaseProperty.managedReferenceValue = newUnityEvent;
            }
            else
            {
                CopyUnityEvent.CopyUnityEvents(unityEventBaseProperty, newUnityEvent);
            }

            unityEventBaseProperty.managedReferenceValue = newUnityEvent;
            viewEventResponses.serializedObject.ApplyModifiedProperties();
        }
    }
}