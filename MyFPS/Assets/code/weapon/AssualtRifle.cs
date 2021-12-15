using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Weapen
{
    public class AssualtRifle:Firearms
    {
        private IEnumerator reloadAmmoCheckerCoroutine;
        private FPMouselook Mouselook;
        
        protected override void Start()
        {
            base.Start();
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
            Mouselook = FindObjectOfType<FPMouselook>();
        }

        protected override void Shooting()
        {
            if (CurrentAmmo <= 0 || !IsAllowShooting()) return;
            MuzzleParticle.Play();
            CurrentAmmo -= 1;
            GunAnimator.Play("Fire", isAiming?1:0,0);

            FireamShootingAudioSource.clip = FirearmsAudioData.ShootingAudioClip;
            FireamShootingAudioSource.Play();
            
            CreateBullet();
            CasingParticle.Play();
            Mouselook.FiringForTest();
            LastFireTime = Time.time;
        }

        protected override void Reload()
        {
            if (CurrentMaxAmmoCarried <= 0 || CurrentAmmo == AmmoInMag) return;
            GunAnimator.Play("Reload", 0, 0);
            
            FireamShootingAudioSource.clip =
                CurrentAmmo > 0 ? FirearmsAudioData.Reloadleft:FirearmsAudioData.ReloadOutOf;
            FireamShootingAudioSource.Play();
            
            if (reloadAmmoCheckerCoroutine == null)
            {
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
            else
            {
                StopCoroutine(reloadAmmoCheckerCoroutine);
                reloadAmmoCheckerCoroutine = null;
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
        }

        /*
         private void Update()
         {
             if (Input.GetMouseButton(0))
             {
                 DoAttack();
             }
             if (Input.GetKeyDown(KeyCode.R))
             {
                 Reload();
             }
             
             // 瞄准
             if (Input.GetMouseButtonDown(1))
             {   
                 GunAnimator.SetLayerWeight(1, 1);
                 isAiming = true;
                 Aim();
             }
             if (Input.GetMouseButtonUp(1))
             {
                 GunAnimator.SetLayerWeight(1, 0);
                 isAiming = false;
                 Aim();
             }
         }
         */
       
        private void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, 
                Muzzlepoint.position, Muzzlepoint.rotation);

            tmp_Bullet.transform.eulerAngles += CalculateSpreadOffset();
            
            var tmp_BulletRigidbody = tmp_Bullet.AddComponent<Rigidbody>();
            tmp_BulletRigidbody.velocity = tmp_Bullet.transform.forward * 200f;
        }
    }
}