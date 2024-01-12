using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Scene
{
    public abstract class SceneChangerBase : MonoBehaviour
    {
        //[SerializeField] private SceneChangeEffect _sceneChangeEffect;
        protected SceneChangeEffect _sceneChangeEffect;

        private void Awake()
        {
            Initialize();
        }

        protected abstract void Initialize();

        /*
        protected void OnEnable()
        {
            SceneManager.AddSceneChanger(this);
        }

        protected void OnDisable()
        {
            SceneManager.RemoveSceneChanger(this);
        }
        */

        public virtual bool InSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (_sceneChangeEffect != effect) return false;
            return true;
        }
        
        public virtual bool OutSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (_sceneChangeEffect != effect) return false;
            return true;
        }
    }
}