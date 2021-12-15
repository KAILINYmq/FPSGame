using UnityEngine;

namespace Scripts.Weapen
{
    [CreateAssetMenu(menuName = "FPS/Firearms Audio Data")]
    public class FirearmsAudioData:ScriptableObject
    {
        public AudioClip ShootingAudioClip;
        public AudioClip Reloadleft;
        public AudioClip ReloadOutOf;
    }
}