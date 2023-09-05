// @BlockGraphWindow.cs
// @brief
// @author ICE
// @date 2023/09/05
// 
// @details

using QBuild.Const;
using UnityEditor;
using UnityEngine;

namespace QBuild.GameDebugWindow.BlockContactView
{
    public class BlockGraphWindow : EditorWindow
    {
        [MenuItem(EditorConst.WindowPrePath + "BlockGraphWindow")]
        private static void ShowWindow()
        {
            var window = GetWindow<BlockGraphWindow>();
            window.titleContent = new GUIContent("BlockGraphWindow");
            window.Show();
        }

        private void CreateGUI()
        {
        }
    }
}