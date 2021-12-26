using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Weapen
{
    public class Bullet : MonoBehaviour
    {
        public float BulletSpeed;
        public GameObject ImpactPrefab;
        public ImpactAudioData ImpactAudioData;
        private Transform bulletTransform;
        private Vector3 prevPosition;
        
        private void Start()
        {
            bulletTransform = transform;
            prevPosition = bulletTransform.position;
        }

        private void Update()
        {
            prevPosition = bulletTransform.position;
            
            bulletTransform.Translate(0, 0, BulletSpeed*Time.deltaTime);
            if (!Physics.Raycast(prevPosition, 
                (bulletTransform.position - prevPosition).normalized,
                out RaycastHit tmp_Hit, (bulletTransform.position- prevPosition).magnitude)) 
                return;
            var tmp_BulletEffect = Instantiate(ImpactPrefab, tmp_Hit.point,
                Quaternion.LookRotation(tmp_Hit.normal, Vector3.up));
            Destroy(tmp_BulletEffect, 3);

            var tmp_tagsWithAudio = ImpactAudioData.ImpactAudioDatas.Find(
                (_audioData) => _audioData.Tag.Equals(tmp_Hit.collider.tag));
            if (tmp_tagsWithAudio == null) return;
            AudioClip tmp_AudioClip = tmp_tagsWithAudio.ImpactAudioClips[Random.Range(0, 
                tmp_tagsWithAudio.ImpactAudioClips.Count)];
            AudioSource.PlayClipAtPoint(tmp_AudioClip, tmp_Hit.point);
        }
    }
}
