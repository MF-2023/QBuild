using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Audio
{
    [CreateAssetMenu(fileName = "AudioManagerSO",menuName = "Data/Audio")]
    public class AudioSO : ScriptableObject
    {
        public float MasterVolume = 0.5f;
        public float BGMVolume = 0.8f;
        public float SEVolume 1f;
    }
}
