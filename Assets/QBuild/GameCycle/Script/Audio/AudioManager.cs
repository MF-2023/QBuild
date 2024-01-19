using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace QBuild.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public AudioManager Instance { get; private set; }
        
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioSource _bgmSource;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            PlayBGM();
        }

        private void PlayBGM()
        {
            _bgmSource.Play();
        }

        private void StopBGM()
        {
            _bgmSource.Stop();
        }

        /// <summary>
        /// マスターボリュームの音量設定
        /// </summary>
        public void SetMasterVolume()
        {
            
        }

        public void SetBGMVolume()
        {
            
        }

        public void SetSEVolume()
        {
            
        }
    }
}
