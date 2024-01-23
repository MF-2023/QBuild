using System.Collections;
using System.Collections.Generic;
using QBuild.Const;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;

namespace QBuild.StageEditor
{
    public abstract class StageDataSceneTab : EditorToolbarButton
    {
        private const string FolderPath = "Assets/QBuild/Editor/StageEditor/";
        private const string SceneName = "StageEditor.unity";
        private const string ButtonName = "ステージエディタシーン";

        [MenuItem(EditorConst.WindowPrePath + "ステージエディタ/" + ButtonName)]
        public static void OpenStageEditorScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(FolderPath + SceneName);
            }
        }
    }
}