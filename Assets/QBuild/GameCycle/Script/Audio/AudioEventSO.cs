using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace QBuild.Audio
{
    [CreateAssetMenu(fileName = "AudioManagerEvent",menuName = "Data/Audio Event")]
    public class AudioEventSO : ScriptableObject
    {
        public Action<float> SetMasterVolumeEvent;
        public Action<float> SetBGMVolumeEvent;
        public Action<float> SetSEVolumeEvent;
    }
}