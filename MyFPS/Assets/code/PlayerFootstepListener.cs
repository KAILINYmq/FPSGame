using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepListener : MonoBehaviour
{
    public FootstepAudioData FootstepAudioData;
    public AudioSource FootstepAudioSource;

    private CharacterController characterController;
    private float nextPlayTime;

    public LayerMask layermask;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (characterController.isGrounded &&
            Physics.Linecast(transform.position, 
                transform.position+Vector3.down *
                (characterController.height / 2 + characterController.skinWidth-characterController.center.y),
                out RaycastHit tmp_HitInfo,
                layermask)
            )
        {
            if (characterController.velocity.normalized.magnitude > 0.1f)
            {
                nextPlayTime += Time.deltaTime;
            
                foreach (var tmp_AudioElement in FootstepAudioData.FootstepAudios) 
                {
                    if (tmp_HitInfo.collider.CompareTag(tmp_AudioElement.Tag) &&
                        nextPlayTime >= tmp_AudioElement.Delay)
                    {
                        int tmp_AudioIndex = UnityEngine.Random.Range(0, 
                            tmp_AudioElement.AudioClips.Count);
                        AudioClip tmp_FotstepAudioClip = tmp_AudioElement.AudioClips[tmp_AudioIndex];
                        FootstepAudioSource.clip = tmp_FotstepAudioClip;
                        FootstepAudioSource.Play();
                        nextPlayTime = 0;
                        break;
                    }
                }
            }
            else
            {
                FootstepAudioSource.Stop();
            }

        }
    }
}
