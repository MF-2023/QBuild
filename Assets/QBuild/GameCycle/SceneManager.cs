using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace QBuild.Scene
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private SceneChangeEffect _startSceneChangeEffect = SceneChangeEffect.Fade;
        [SerializeField] private float _startSceneChangeTime = 1.0f;

        private static SceneManager _instance = null;
        private bool _loadedScene = false;

        [SerializeField] private List<SceneChangerBase> _sceneChangers = new List<SceneChangerBase>();

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
            else if (_instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            InSCStart(_startSceneChangeTime, _startSceneChangeEffect);
        }

        /// <summary>
        /// すぐにシーンの変更
        /// </summary>
        /// <param name="index">シーン番号</param>
        public static void LoadScene(int index)
        {
            if (!CheckInstance() || _instance._loadedScene) return;
            _instance.StartLoadScene(index);
        }
        
        /// <summary>
        /// 指定した時間後にシーンを変更
        /// </summary>
        /// <param name="scChangeTime">シーンエフェクトの再生時間</param>
        /// <param name="index">シーン番号</param>
        /// <param name="scEffect">使用するシーンチェンジエフェクト</param>
        /// <param name="waitTime">暗転後に待つ時間(デフォルト　2秒)</param>
        public static void ChangeSceneWait(int index, SceneChangeEffect scEffect = SceneChangeEffect.Fade,
            float scChangeTime = 2.0f, float waitTime = 2.0f)
        {
            if (!CheckInstance()) return;
            _instance.StartChangeSceneWait(scChangeTime, waitTime, index, scEffect);
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

        private void StartChangeSceneWait(float scChangeTime, float waitTime, int index, SceneChangeEffect scEffect)
        {
            StartCoroutine(changeSceneWait(scChangeTime, waitTime, index, scEffect));
        }

        private IEnumerator changeSceneWait(float scChangeTime, float waitTime, int index, SceneChangeEffect scEffect)
        {
            if (_loadedScene) yield break;

            //シーンの切り替えエフェクト
            OutSCStart(scChangeTime, scEffect);
            UnityEngine.SceneManagement.Scene delScene = UnitySceneManager.GetActiveScene();

            _loadedScene = true;
            yield return new WaitForSeconds(scChangeTime + waitTime);

            //シーンの切り替え処理
            //var unloadAsync = UnitySceneManager.UnloadSceneAsync(UnitySceneManager.GetActiveScene());
            var unloadAsync = UnitySceneManager.UnloadSceneAsync(delScene);
            yield return unloadAsync;

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

            yield return async;

            //シーンの切り替えエフェクト
            InSCStart(scChangeTime, scEffect);
            _loadedScene = false;
        }
        
        private async UniTask changeSceneWaitUni(float scChangeTime, float waitTime, int index, SceneChangeEffect scEffect)
        {
            if (_loadedScene) return;
            OutSCStart(scChangeTime,scEffect);
            
            UnityEngine.SceneManagement.Scene delScene = UnitySceneManager.GetActiveScene();
            await UniTask.WaitUntil(() => UnitySceneManager.GetSceneByBuildIndex(index).isLoaded);
            await UniTask.WaitUntil(() => delScene.isLoaded == false);

            InSCStart(scChangeTime, scEffect);
        }

        private static bool CheckInstance()
        {
            if (_instance == null)
            {
                Debug.LogError("SceneManagerが存在しません。");
                return false;
            }

            return true;
        }

        private void InSCStart(float scTime, SceneChangeEffect effect)
        {
            foreach (var sceneChanger in _sceneChangers)
            {
                sceneChanger.InSCEffect(scTime, effect);
            }
        }

        private void OutSCStart(float scTime, SceneChangeEffect effect)
        {
            foreach (var sceneChanger in _sceneChangers)
            {
                sceneChanger.OutSCEffect(scTime, effect);
            }
        }

        public static void AddSceneChanger(SceneChangerBase sceneChanger)
        {
            if (_instance == null) return;
            if (!_instance._sceneChangers.Contains(sceneChanger)) _instance._sceneChangers.Add(sceneChanger);
        }

        public static void RemoveSceneChanger(SceneChangerBase sceneChanger)
        {
            if (_instance == null) return;
            if (_instance._sceneChangers.Contains(sceneChanger)) _instance._sceneChangers.Remove(sceneChanger);
        }
    }

    public enum SceneChangeEffect
    {
        Fade,
        LoadPanel
    }
}