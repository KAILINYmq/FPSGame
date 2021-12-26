using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Weapen
{
    [CreateAssetMenu(menuName = "FPS/Impact Audio Data")]
    public class ImpactAudioData : ScriptableObject
    {
        public List<ImpactagsWithAudio> ImpactAudioDatas;
    }
    
    [System.Serializable]
    public class ImpactagsWithAudio
    {
        public string Tag;
        public List<AudioClip> ImpactAudioClips;
    }
}
