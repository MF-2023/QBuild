using System.Collections;
using System.Collections.Generic;
using QBuild.Scene;
using UnityEngine;

namespace QBuild.GameCycle
{
    public class LoadPanel : SceneChangerBase
    {
        [SerializeField] private GameObject _loadPanel;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        protected override void Initialize()
        {
            _sceneChangeEffect = SceneChangeEffect.LoadPanel;
            _loadPanel.SetActive(false);
        }
        
        public override bool InSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (!base.InSCEffect(scTime, effect)) return false;
            HideLoadPanel();

            return true;
        }
        
        public override bool OutSCEffect(float scTime, SceneChangeEffect effect)
        {
            if (!base.OutSCEffect(scTime, effect)) return false;
            ShowLoadPanel();

            return true;
        }

        private void ShowLoadPanel()
        {
            _canvasGroup.gameObject.SetActive(true);
            _loadPanel.SetActive(true);

        }

        private void HideLoadPanel()
        {
            _canvasGroup.gameObject.SetActive(false);
            _loadPanel.SetActive(false);
        }
    }
}
