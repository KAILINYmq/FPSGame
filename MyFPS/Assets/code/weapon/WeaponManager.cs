using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Items;
using UnityEngine;
using Scripts.Weapen;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
   public Firearms MainWeapon;
   public Firearms ScendaryWeapon;
   public GameObject UI;
   public CroshairUI croshairUI;
   public Text AmmoCounttextlabel;
   public Transform WorldCameraTransform;
   public float RaycastmaxDistance = 2;
   public LayerMask CheckItemlayermask;
   public List<Firearms> Arms = new List<Firearms>();
   
   private Firearms carriedWeapon;
   private FPCharacterController fpCharacterController;
   private IEnumerator waitingForHolsterEndCoroutine;
   private void Start()
   {
      Debug.Log($"Current weapon is null ?{carriedWeapon == null}");
      croshairUI = UI.GetComponent<CroshairUI>();
      fpCharacterController = FindObjectOfType<FPCharacterController>();
      if (MainWeapon)
      {
         carriedWeapon = MainWeapon;
         fpCharacterController.SetupAnimator(carriedWeapon.GunAnimator);
      }
   }


   private void Update()
   {
      CheckItem();
      if (!carriedWeapon) return;
      SwapWeapon();
      AmmoCounttextlabel.text = carriedWeapon.GetCurrentAmmo.ToString()+" / "+
                                carriedWeapon.GetCurrentMaxAmmoCarried.ToString();
      
      if (Input.GetMouseButton(0))
      {
         carriedWeapon.HoldTrigger();
         croshairUI.IsFire = true;
      }
      if (Input.GetMouseButtonUp(0))
      {
         // DoAttack();
         croshairUI.IsFire = false;
         carriedWeapon.ReleaseTrigger();
      }
      if (Input.GetKeyDown(KeyCode.R))
      {
         carriedWeapon.ReloadAmmo();
      }
                   
      // 瞄准
      if (Input.GetMouseButtonDown(1))
      {  
         carriedWeapon.GunAnimator.SetLayerWeight(1, 1);
         //  isAiming = true;
         //  Aim();
         carriedWeapon.Aiming(true);
         UI.SetActive(false);
      }
      if (Input.GetMouseButtonUp(1))
      {
         carriedWeapon.GunAnimator.SetLayerWeight(1, 0);
         // isAiming = false;
         // Aim();
         carriedWeapon.Aiming(false);
         UI.SetActive(true);
      }
   }

   private void CheckItem()
   {
      if (Physics.Raycast(WorldCameraTransform.position,
         WorldCameraTransform.forward, out RaycastHit tmp_RaycastHit,
         RaycastmaxDistance, CheckItemlayermask) && 
          Input.GetKeyDown(KeyCode.E))
      {
         if (tmp_RaycastHit.collider.TryGetComponent(out BaseItem tmp_BaseItem))
         {
            Debug.Log(tmp_RaycastHit.collider.name);
            PickupWeapon(tmp_BaseItem);
            // todo 倍镜bug
            PickupAttachment(tmp_BaseItem);
         }
      }
   }

   private void PickupWeapon(BaseItem tmp_BaseItem)
   {
      if (tmp_BaseItem is FirearmsItem tmp_FirearmsItem)
      {
         foreach (Firearms tmp_Arm in Arms)
         {
            if (tmp_FirearmsItem.ArmsName.CompareTo(tmp_Arm.name) == 0)
            {
               switch (tmp_FirearmsItem.CurremtFirearmsType)
               {
                  case FirearmsItem.FirearmsType.AssultRefile:
                     MainWeapon = tmp_Arm;
                     break;
                  case FirearmsItem.FirearmsType.HandGun:
                     ScendaryWeapon = tmp_Arm;
                     break;
                  default:
                     throw new ArgumentOutOfRangeException();
               }
               SetupCarriedWeapon(tmp_Arm);
            }
         }
      } 
   }

   private void PickupAttachment(BaseItem tmp_BaseItem)
   {  
      if (!(tmp_BaseItem is AttachmentItem tmp_AttachmentItem)) return;
      Debug.Log(tmp_BaseItem);
      switch (tmp_AttachmentItem.CurrentAttachmentType)
      {
         case AttachmentItem.AttachmentType.Scope:
            foreach (ScopeInfo tmp_ScopeInfo in carriedWeapon.ScopeInfos)
            {
               if (tmp_ScopeInfo.ScopeName.CompareTo(tmp_AttachmentItem.ItemName) != 0)
               {
                  tmp_ScopeInfo.ScopeGameObject.SetActive(false);
                  continue;
               }
               tmp_ScopeInfo.ScopeGameObject.SetActive(true);
               carriedWeapon.BaseIronSight.ScopeGameObject.SetActive(false);
               carriedWeapon.SetupCarriedScope(tmp_ScopeInfo);
            }
            break;
         case AttachmentItem.AttachmentType.Other:
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }

   private void SwapWeapon()
   {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         if (MainWeapon == null) return;
         /*carriedWeapon.gameObject.SetActive(false);
         carriedWeapon = MainWeapon;
         carriedWeapon.gameObject.SetActive(true);
         fpCharacterController.SetupAnimator(carriedWeapon.GunAnimator);
         */
         if (carriedWeapon == MainWeapon) return;
         if (carriedWeapon.gameObject.activeInHierarchy)
         {
            StartWaitingForHolsterEndCoroutine();
            carriedWeapon.GunAnimator.SetTrigger("holster");
         }
         else
         {
            SetupCarriedWeapon(MainWeapon);
         }
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         if (ScendaryWeapon == null) return;
         /*
         carriedWeapon.gameObject.SetActive(false);
         carriedWeapon = ScendaryWeapon;
         carriedWeapon.gameObject.SetActive(true);
         fpCharacterController.SetupAnimator(carriedWeapon.GunAnimator);
         */
         if (carriedWeapon == ScendaryWeapon) return;
         if (carriedWeapon.gameObject.activeInHierarchy)
         {
            StartWaitingForHolsterEndCoroutine();
            carriedWeapon.GunAnimator.SetTrigger("holster");
         }
         else
         {
            SetupCarriedWeapon(ScendaryWeapon);
         }
      }
   }

   private void StartWaitingForHolsterEndCoroutine()
   {
      if (waitingForHolsterEndCoroutine == null)
         waitingForHolsterEndCoroutine = WaitingForHolsterEnd();
      StartCoroutine(waitingForHolsterEndCoroutine);
   }

   private IEnumerator WaitingForHolsterEnd()
   {
      while (true)
      {
         AnimatorStateInfo tmp_AnimatorStateInfo = carriedWeapon.GunAnimator.GetCurrentAnimatorStateInfo(0);
         if (tmp_AnimatorStateInfo.IsTag("holster") &&
             tmp_AnimatorStateInfo.normalizedTime>=0.9f)
         {  
            // 隐藏現在的武器
            // 显示目标武器
            SetupCarriedWeapon(carriedWeapon == MainWeapon? ScendaryWeapon:MainWeapon);
            waitingForHolsterEndCoroutine = null;
            fpCharacterController.SetupAnimator(carriedWeapon.GunAnimator);
            yield break;
         }

         yield return null;
      }
   }

   private void SetupCarriedWeapon(Firearms _targetWeapon)
   {
      if (carriedWeapon) carriedWeapon.gameObject.SetActive(false);
      carriedWeapon = _targetWeapon;
      carriedWeapon.gameObject.SetActive(true);
   }
}

