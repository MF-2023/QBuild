using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Scene
{
    public abstract class SceneChangerBase : MonoBehaviour
    {
        [SerializeField] private SceneChangeEffect _sceneChangeEffect;

        protected void OnEnable()
        {
            SceneManager.AddSceneChanger(this);
        }

        protected void OnDisable()
        {
            SceneManager.RemoveSceneChanger(this);
        }

        public virtual void InSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (_sceneChangeEffect != effect) return;
        }
        
        public virtual void OutSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (_sceneChangeEffect != effect) return;
        }
    }
}