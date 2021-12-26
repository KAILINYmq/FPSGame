using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Scripts.Weapen
{
    public abstract class Firearms:MonoBehaviour,IWeapon
    {   
        public GameObject BulletImpactPrefab;
        public GameObject BulletPrefab;
        public Camera EyeCamera;
        public Camera GunCamera;
        public float SpreadAngle;
        
        public Transform Muzzlepoint;
        public Transform CasingPoint;
        public ParticleSystem MuzzleParticle;
        public ParticleSystem CasingParticle;

        public float FireRate;
        public int AmmoInMag = 30;
        public int MaxAmmoCarried = 120;
        
        public AudioSource FireamShootingAudioSource;
        public AudioSource FireamsReloadAudioSource;
        public FirearmsAudioData FirearmsAudioData;
        public ImpactAudioData ImpactAudioData;
        
        public List<ScopeInfo> ScopeInfos;
        public ScopeInfo BaseIronSight;
        protected ScopeInfo rigoutScopeInfo; 

        public int GetCurrentAmmo => CurrentAmmo;
        public int GetCurrentMaxAmmoCarried => CurrentMaxAmmoCarried;
        
        internal Animator GunAnimator;
        protected int CurrentAmmo;
        protected int CurrentMaxAmmoCarried;
        protected float LastFireTime;
        protected AnimatorStateInfo GunStateInfo;
        protected float OriginFOV;
        protected bool isAiming;
        protected bool IsHoldingTrigger;
        private IEnumerator doAimCoroutine;
        protected float EyeOriginFOV;
        protected float GunOriginFOV;
        protected Vector3 originalEyePosition;
        protected Transform gunCameraTransform;
        
        protected virtual void Awake()
        {
            CurrentAmmo = AmmoInMag;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            GunAnimator = GetComponent<Animator>();
            OriginFOV = EyeCamera.fieldOfView;
            doAimCoroutine = DoAim();
            EyeOriginFOV = EyeCamera.fieldOfView;
            GunOriginFOV = GunCamera.fieldOfView;
            gunCameraTransform = GunCamera.transform;
            originalEyePosition = gunCameraTransform.localPosition;
            rigoutScopeInfo = BaseIronSight;
        }

        public void DoAttack()
        {
            Shooting();
        }

        protected abstract void Shooting();
        protected abstract void Reload();

       // protected abstract void Aim();
        protected bool IsAllowShooting()
        {
            return Time.time-LastFireTime > 1/FireRate;
        }
        
        protected Vector3 CalculateSpreadOffset()
        {
            float tmp_SpreadPercent = SpreadAngle / EyeCamera.fieldOfView;
            return tmp_SpreadPercent * UnityEngine.Random.insideUnitCircle;
        }
        
        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while (true)
            {
                yield return null;
                GunStateInfo = GunAnimator.GetCurrentAnimatorStateInfo(0);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        var num = AmmoInMag - CurrentAmmo;
                        if (CurrentMaxAmmoCarried <= AmmoInMag)
                        {
                            CurrentAmmo = CurrentMaxAmmoCarried;
                            CurrentMaxAmmoCarried = 0;
                        }
                        else
                        {
                            CurrentAmmo = AmmoInMag;
                            CurrentMaxAmmoCarried -= num;
                        }
                        yield break;
                    }
                }
            }
        }
        
        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;
                
                float tmp_CurrentFOV = 0;
                EyeCamera.fieldOfView = 
                    Mathf.SmoothDamp(EyeCamera.fieldOfView, 
                        isAiming?rigoutScopeInfo.GunFov:EyeOriginFOV, 
                        ref tmp_CurrentFOV, Time.deltaTime*2);

                float tmp_GunCurrentFOV = 0;
                GunCamera.fieldOfView = Mathf.SmoothDamp(GunCamera.fieldOfView,
                    isAiming?rigoutScopeInfo.GunFov:GunOriginFOV,
                    ref tmp_CurrentFOV, Time.deltaTime*2);

                Vector3 tmp_RefPosition = Vector3.zero;
                gunCameraTransform.localPosition = Vector3.SmoothDamp(gunCameraTransform.localPosition,
                    isAiming ? rigoutScopeInfo.EyePosition : originalEyePosition,
                    ref tmp_RefPosition, Time.deltaTime*2);
            }
        }
        
        internal void Aiming(bool _isAiming)
        {
            isAiming = _isAiming;
            GunAnimator.SetBool("Aim", isAiming);
            if (doAimCoroutine == null)
            {
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
            else
            {
                StopCoroutine(doAimCoroutine);
                doAimCoroutine = DoAim();
                StartCoroutine(doAimCoroutine);
            }
        }

        internal void SetupCarriedScope(ScopeInfo _scopeInfo)
        {
            rigoutScopeInfo = _scopeInfo;
        }

        internal void HoldTrigger()
        {
            DoAttack();
            IsHoldingTrigger = true;
        }

        internal void ReleaseTrigger()
        {
            IsHoldingTrigger = false;
        }

        internal void ReloadAmmo()
        {
            Reload();
        }
    }

    [System.Serializable]
    public class ScopeInfo
    {
        public string ScopeName;
        public GameObject ScopeGameObject;
        public float EyeFov;
        public float GunFov;
        public Vector3 EyePosition;
    }
}