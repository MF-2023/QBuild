using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QBuild.Audio
{
    [CreateAssetMenu(fileName = "AudioManagerSO",menuName = "Data/Audio")]
    public class AudioSO : ScriptableObject
    {
        public float MasterVolume;
        public float BGMVolume;
        public float SEVolume;
    }
}
