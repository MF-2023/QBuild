using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QBuild.Scene
{
    public class SetupSceneCreate
    {
        [RuntimeInitializeOnLoadMethod]
        private static void CreateSetupScene()
        {
            string sceneName = "SetupScene";
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }
}
