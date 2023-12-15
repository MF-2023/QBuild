using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
namespace  QBuild.Scene
{
    public class SceneManager : MonoBehaviour
    {
        private static SceneManager _instance = null;
        private bool _loadedScene = false;
        
        private List<SceneChangerBase> _sceneChangers = new List<SceneChangerBase>(); 
        
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
        /// �����ɃV�[���̕ύX
        /// </summary>
        /// <param name="index">�V�[���ԍ�</param>
        public static void LoadScene(int index)
        {
            if (!CheckInstance() || _instance._loadedScene) return;
            _instance.StartLoadScene(index);
        }

        /// <summary>
        /// �w�肵�����Ԍ�ɃV�[����ύX(�t�F�[�h���g�p)
        /// </summary>
        /// <param name="waitTime">�҂���</param>
        /// <param name="index">�V�[���ԍ�</param>
        public static void ChangeSceneWait(float waitTime, int index)
        {
            if (!CheckInstance()) return;
            _instance.StartChangeSceneWait(waitTime,index);
        }
        
        /// <summary>
        /// �w�肵�����Ԍ�ɃV�[����ύX
        /// </summary>
        /// <param name="waitTime">�҂���</param>
        /// <param name="index">�V�[���ԍ�</param>
        /// <param name="scEffect">�g�p����V�[���`�F���W�G�t�F�N�g</param>
        public static void ChangeSceneWait(float waitTime, int index, SceneChangeEffect scEffect)
        {
            if (!CheckInstance()) return;
            _instance.StartChangeSceneWait(waitTime, index);
        }

        private void StartLoadScene(int index)
        {
            UnitySceneManager.LoadScene(index, LoadSceneMode.Additive);
            var unloadAsync = UnitySceneManager.UnloadSceneAsync(UnitySceneManager.GetActiveScene());
            unloadAsync.completed += (async) =>
            {
                for (int i = 0; i < UnitySceneManager.sceneCount; i++)
                {
                    UnityEngine.SceneManagement.Scene scene = UnitySceneManager.GetSceneAt(i);
                    if (scene.buildIndex == index)
                    {
                        UnitySceneManager.SetActiveScene(scene);
                        break;
                    }
                }
            };
        }
        
        private void StartChangeSceneWait(float waitTime, int index)
        {
            StartCoroutine(changeSceneWait(waitTime,index));
        }

        private IEnumerator changeSceneWait(float waitTime , int index)
        {
            if (_loadedScene) yield break;

            //�V�[���̐؂�ւ�����
            UnitySceneManager.UnloadSceneAsync(UnitySceneManager.GetActiveScene());
            var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            async.completed += (async) =>
            {
                for (int i = 0; i < UnitySceneManager.sceneCount; i++)
                {
                    UnityEngine.SceneManagement.Scene scene = UnitySceneManager.GetSceneAt(i);
                    if (scene.buildIndex == index)
                    {
                        UnitySceneManager.SetActiveScene(scene);
                        break;
                    }
                }
            };
            
            _loadedScene = true;
            async.allowSceneActivation = false;
            yield return new WaitForSeconds(waitTime);
            _loadedScene = false;
            async.allowSceneActivation = true;
        }
        
        private static bool CheckInstance()
        {
            if (_instance == null)
            {
                Debug.LogError("SceneManager�����݂��܂���B");
                return false;
            }

            return true;
        }
        
        public static void AddSceneChanger(SceneChangerBase sceneChanger)
        {
            if (_instance == null) return;
            if(!_instance._sceneChangers.Contains(sceneChanger)) _instance._sceneChangers.Add(sceneChanger);
        }
        
        public static void RemoveSceneChanger(SceneChangerBase sceneChanger)
        {
            if (_instance == null) return;
            if (_instance._sceneChangers.Contains(sceneChanger)) _instance._sceneChangers.Remove(sceneChanger);
        }
    }

    public enum SceneChangeEffect
    {
        Fade
    }
}
