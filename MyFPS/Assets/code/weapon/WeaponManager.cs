using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Weapen;

public class WeaponManager : MonoBehaviour
{
   public Firearms MainWeapon;
   public Firearms ScendaryWeapon;


   private Firearms carriedWeapon;
   
   private void Start()
   {
      carriedWeapon = MainWeapon;
   }

   private void Update() 
   {  
      if (!carriedWeapon) return;
      SwapWeapon();
      if (Input.GetMouseButton(0))
      {
         carriedWeapon.HoldTrigger();
      }
      if (Input.GetMouseButtonUp(0))
      {
         // DoAttack();
         carriedWeapon.ReleaseTrigger();
      }
      if (Input.GetKeyDown(KeyCode.R))
      {
         carriedWeapon.ReloadAmmo();
      }
                   
      // 瞄准
      if (Input.GetMouseButtonDown(1))
      {
         //  GunAnimator.SetLayerWeight(1, 1);
         //  isAiming = true;
         //  Aim();
         carriedWeapon.Aiming(true);
      }
      if (Input.GetMouseButtonUp(1))
      {
         // GunAnimator.SetLayerWeight(1, 0);
         // isAiming = false;
         // Aim();
         carriedWeapon.Aiming(false);
      }
   }

   private void SwapWeapon()
   {
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         carriedWeapon.gameObject.SetActive(false);
         carriedWeapon = MainWeapon;
         carriedWeapon.gameObject.SetActive(true);
      }
      else if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         carriedWeapon.gameObject.SetActive(false);
         carriedWeapon = ScendaryWeapon;
         carriedWeapon.gameObject.SetActive(true);
      }
   }
}

