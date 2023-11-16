using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEditor.Toolbars;
using Object = UnityEngine.Object;

namespace QBuild.LevelEditor
{
    // オーバーレイ本体
    [Overlay(typeof(SceneView), MenuPath)]
    public class SceneControlExample : ToolbarOverlay
    {
        const string MenuPath = "Custom/SceneControl";

        SceneControlExample() : base(
            SelectPlayerButton.ID // IDを使ってUIを登録
        )
        {
        }
    }

    // ボタンの挙動 
    [EditorToolbarElement(ID, typeof(SceneView))]
    public class SelectPlayerButton : ToolbarButton
    {
        public const string ID = "SeceneControlExample.SelectPlayerButton"; // ユニークなID

        SelectPlayerButton()
        {
            tooltip = "Playerタグが設定されているオブジェクトを選択します。";
            text = "プレイヤーを選択";
            clicked += OnClicked;
        }

        private void OnClicked()
        {
            Selection.activeGameObject = GameObject.FindWithTag("Player");
        }
    }

    enum IconSkin
    {
        Default,
        Light,
        Pro
    };

    static class IconUtility
    {
        static Dictionary<string, Texture2D> s_Icons = new();
        static string s_IconFolderPath = "Content/Icons/";

        static T LoadInternalAsset<T>(string path) where T : Object
        {
            string full = string.Format("{0}{1}", "Packages/com.unity.probuilder/", path);
            return AssetDatabase.LoadAssetAtPath<T>(full);
            ;
        }

        /// <summary>
        /// Load an icon from the ProBuilder/Icons folder. IconName must *not* include the extension or `_Light` mode suffix.
        /// </summary>
        /// <param name="iconName"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static Texture2D GetIcon(string iconName, IconSkin skin = IconSkin.Default)
        {
#if PB_DEBUG
            if (iconName.EndsWith(".png"))
                pb_Log.Error("GetIcon(string) called with .png suffix!");

            if (iconName.EndsWith("_Light"))
                pb_Log.Error("GetIcon(string) called with _Light suffix!");
#endif

            bool isDarkSkin = skin == IconSkin.Default ? EditorGUIUtility.isProSkin : skin == IconSkin.Pro;
            string name = isDarkSkin ? iconName : iconName + "_Light";

            Texture2D icon = null;

            if (!s_Icons.TryGetValue(name, out icon))
            {
                int i = 0;

                do
                {
                    // if in light mode:
                    // - do one lap searching for light in 2x first and then in normal if no 2X found
                    // - if nothing found, next searching for default
                    string fullPath;
                    if (EditorGUIUtility.pixelsPerPoint > 1)
                    {
                        fullPath = string.Format("{0}{1}@2x.png", s_IconFolderPath, i == 0 ? name : iconName);
                        icon = LoadInternalAsset<Texture2D>(fullPath);
                    }

                    if (icon == null)
                    {
                        fullPath = string.Format("{0}{1}.png", s_IconFolderPath, i == 0 ? name : iconName);
                        icon = LoadInternalAsset<Texture2D>(fullPath);
                    }
                } while (!isDarkSkin && ++i < 2 && icon == null);

                s_Icons.Add(name, icon);
            }

            return icon;
        }
    }

    public class LevelEditorWindow : EditorWindow
    {
        [MenuItem("Window/LevelEditorWindow")]
        private static void Init()
        {
            var window = (LevelEditorWindow) EditorWindow.GetWindow(typeof(LevelEditorWindow));
            window.Show();
        }

        internal static readonly Color TEXT_COLOR_WHITE_NORMAL = new Color(0.82f, 0.82f, 0.82f, 1f);
        internal static readonly Color TEXT_COLOR_WHITE_HOVER = new Color(0.7f, 0.7f, 0.7f, 1f);
        internal static readonly Color TEXT_COLOR_WHITE_ACTIVE = new Color(0.5f, 0.5f, 0.5f, 1f);

        private bool _hover = false;

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
        {
            var s_RowStyleVertical = new GUIStyle();
            s_RowStyleVertical.alignment = TextAnchor.MiddleLeft;
            s_RowStyleVertical.stretchWidth = true;
            s_RowStyleVertical.stretchHeight = false;
            s_RowStyleVertical.margin = new RectOffset(0, 0, 0, 0);
            s_RowStyleVertical.padding = new RectOffset(0, 0, 0, 0);
            GUILayout.BeginHorizontal(s_RowStyleVertical);

            GUI.backgroundColor = new Color(0.6666f, 0.4f, 0.2f, 1f);
            var s_ButtonStyleVertical = new GUIStyle();

            s_ButtonStyleVertical.normal.background = IconUtility.GetIcon("Toolbar/Button_Normal", IconSkin.Pro);
            s_ButtonStyleVertical.normal.textColor = EditorGUIUtility.isProSkin ? TEXT_COLOR_WHITE_NORMAL : Color.black;
            s_ButtonStyleVertical.hover.background = IconUtility.GetIcon("Toolbar/Button_Hover", IconSkin.Pro);
            s_ButtonStyleVertical.hover.textColor = EditorGUIUtility.isProSkin ? Color.blue : Color.blue;
            s_ButtonStyleVertical.active.background = IconUtility.GetIcon("Toolbar/Button_Pressed", IconSkin.Pro);
            s_ButtonStyleVertical.active.textColor = EditorGUIUtility.isProSkin ? TEXT_COLOR_WHITE_ACTIVE : Color.black;

            s_ButtonStyleVertical.alignment = TextAnchor.MiddleLeft;
            s_ButtonStyleVertical.border = new RectOffset(4, 0, 0, 0);
            s_ButtonStyleVertical.stretchWidth = true;
            s_ButtonStyleVertical.stretchHeight = false;
            s_ButtonStyleVertical.margin = new RectOffset(4, 5, 4, 4);
            s_ButtonStyleVertical.padding = new RectOffset(8, 0, 2, 2);

            if (GUILayout.Button("Test", s_ButtonStyleVertical))
            {
                Debug.Log("Test");
            }

            GUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;


            Rect buttonRect = GUILayoutUtility.GetLastRect();
            bool windowContainsMouse = (this == EditorWindow.mouseOverWindow);
            Event evt = Event.current;
            var m_WantsRepaint = EditorWindow.mouseOverWindow == this && evt.type == EventType.MouseMove;

            if (windowContainsMouse &&
                evt.type != EventType.Layout &&
                !_hover &&
                buttonRect.Contains(evt.mousePosition))
            {
                _hover = true;
                m_WantsRepaint = true;
            }



            if (m_WantsRepaint)
                Repaint();
        }
    }
}