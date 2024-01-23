using System;
using System.Collections;
using System.Collections.Generic;
using QBuild.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace  QBuild.GameCycle.Title
{
    public class OptionPanel : MonoBehaviour
    {
        [SerializeField] private Slider _masterVolumeSlider;
        [SerializeField] private Slider _BGMVolulmeSlider;
        [SerializeField] private Slider _SEVolumeSlider;

        [SerializeField] private AudioSO _audioSO;
        [SerializeField] private AudioEventSO _audioEventSO;

        private void Start()
        {
            _masterVolumeSlider.value = _audioSO.MasterVolume;
            _BGMVolulmeSlider.value = _audioSO.BGMVolume;
            _SEVolumeSlider.value = _audioSO.SEVolume;
            
            //_masterVolumeSliderが変更されたときのイベント追加
            _masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            _BGMVolulmeSlider.onValueChanged.AddListener(SetBGMVolume);
            _SEVolumeSlider.onValueChanged.AddListener(SetSEVolume);
        }

        private void SetMasterVolume(float volume)
        {
            _audioEventSO.SetMasterVolumeEvent?.Invoke(volume);
        }

        private void SetBGMVolume(float volume)
        {
            _audioEventSO.SetBGMVolumeEvent?.Invoke(volume);
        }

        private void SetSEVolume(float volume)
        {
            _audioEventSO.SetSEVolumeEvent?.Invoke(volume);
        }
    }
}
