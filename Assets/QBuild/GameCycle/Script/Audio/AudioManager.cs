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
        [SerializeField] private AudioSO _audioSO;
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
            SetMasterVolume(_audioSO.MasterVolume);
            SetBGMVolume(_audioSO.BGMVolume);
            SetSEVolume(_audioSO.SEVolume);

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
        /// <param name="volume">設定する音量 (0～1)</param>
        public void SetMasterVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _audioSO.MasterVolume = volume;

            float vol = (volume * 100f) - 80f;
            SetVolume("Master", vol);
        }

        /// <summary>
        /// BGMボリュームの音量設定
        /// </summary>
        /// <param name="volume">設定する音量（0～1）</param>
        public void SetBGMVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _audioSO.BGMVolume = volume;

            float vol = (volume * 100f) - 80f;
            SetVolume("BGM", vol);
        }

        /// <summary>
        /// SEボリュームの音量設定
        /// </summary>
        /// <param name="volume">設定する音量（0～1）</param>
        public void SetSEVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            _audioSO.SEVolume = volume;

            float vol = (volume * 100f) - 80f;
            SetVolume("SE", vol);
        }

        private void SetVolume(string paramName,float volume)
        {
            _audioMixer.SetFloat(paramName, volume);
        }
    }
}
