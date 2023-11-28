using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace SoVariableTool.ScriptableEvent
{
    [CustomEditor(typeof(EventListener))]
    public class EventListenerDrawer : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            
            serializedObject.Update();
            // インスペクター UI のルートとなる新しい VisualElement を作成します。
            VisualElement myInspector = new VisualElement();
            
            // _eventResponsesをEventResponseDrawerで表示する
            var myPropertyField = new PropertyField(serializedObject.FindProperty("_eventResponses"));

            ListView listView = new ListView();
            
            var eventListener = this.target as EventListener;
            
            // ListViewのデータの設定
            listView.itemsSource = eventListener._eventResponses;

            // テンプレートを使用してアイテムを表示する方法の定義
            listView.makeItem = () =>
            {
                var label = new Label("I am a list item");
                return label;
            };
            // 簡単なラベルを加えます。
            myInspector.Add(new Label("This is a custom inspector"));
            myInspector.Add(myPropertyField);
            
            // インスペクター UI を返します。
            return myInspector;
        }
    }
}