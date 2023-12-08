using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace  QBuild.Scene
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager _instance = null;
        private bool _canChangeScene = true;
        private bool _loadedScene = false;
        
        public bool CanChangeScene
        {
            get { return _canChangeScene; }
            set { _canChangeScene = value; }
        }
        
        public bool LoadedScene
        {
            get { return _loadedScene; }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if(_instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// すぐにシーンの変更
        /// </summary>
        /// <param name="index">シーン番号</param>
        public static void LoadScene(int index)
        {
            CreateSceneManagerobject();

            if (_instance.LoadedScene) return;
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }

        /// <summary>
        /// 指定した時間後にシーンを変更
        /// </summary>
        /// <param name="waitTime">待つ時間</param>
        /// <param name="index">シーン番号</param>
        public static void ChangeSceneWait(float waitTime, int index)
        {
            CreateSceneManagerobject();
            _instance.StartChangeSceneWait(waitTime,index);
        }

        /// <summary>
        /// シーンを読み込んでおく
        /// </summary>
        /// <param name="index">シーン番号</param>
        public static void LoadSceneWait(int index)
        {
            CreateSceneManagerobject();
            _instance.StartLoadSceneWait(index);
        }

        private void StartChangeSceneWait(float waitTime, int index)
        {
            StartCoroutine(changeSceneWait(waitTime,index));
        }

        private void StartLoadSceneWait(int index)
        {
            StartCoroutine(loadSceneWait(index));
        }

        private IEnumerator changeSceneWait(float waitTime , int index)
        {
            if (_loadedScene) yield break;

            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
            _loadedScene = true;
            async.allowSceneActivation = false;
            yield return new WaitForSeconds(waitTime);
            _loadedScene = false;
            async.allowSceneActivation = true;
        }

        private IEnumerator loadSceneWait(int index)
        {
            if (_loadedScene) yield break;
            
            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
            async.allowSceneActivation = _canChangeScene;

            _loadedScene = true;
            while (!_canChangeScene)
            {
                yield return new WaitForEndOfFrame();
            }

            async.allowSceneActivation = _canChangeScene;
            _loadedScene = false;
        }

        private static SceneManager CreateSceneManagerobject()
        {
            if (_instance != null) return _instance;
            
            GameObject createScecneManager = new GameObject();
            createScecneManager.name = "SceneManager";
            _instance = createScecneManager.AddComponent<SceneManager>();
            return _instance;
        }
    }
}
