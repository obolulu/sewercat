using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Items;
using _Project._Scripts.PlayerScripts;
using _Project._Scripts.ScriptBases;
using DG.Tweening;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private WeaponData currentWeaponData;
    [SerializeField] private WeaponBase currentWeapon;
    [SerializeField] private PlayerController playerController;
    
    [Header("animation settings")]
    [SerializeField] private Vector3 hiddenPosition    = new Vector3(0, -0.5f, 0); // Off-screen position

    [SerializeField] private Vector3 visiblePosition = new Vector3(0, 0.5f, 0);//Vector3.zero;           // On-screen position
    [SerializeField] private float   animationDuration = 0.5f;    
    
    private void Awake()
    {
        InputManager.LeftClickDown  += Attack;
        InputManager.RightClickDown += OnRightClickDown;
        InputManager.RightClickUp   += OnRightClickUp;
        InputManager.SpecialPressed += Special;
        InputManager.PutWeaponDown  += UnequipWeapon;
    } 
    
    private void OnDestroy()
    {
        InputManager.LeftClickDown  -= Attack;
        InputManager.PutWeaponDown  -= UnequipWeapon;
        InputManager.RightClickDown -= OnRightClickDown;
        InputManager.RightClickUp   -= OnRightClickUp;
        InputManager.SpecialPressed -= Special;
    }

    #region  Right Click

    private void OnRightClickDown()
    {
        currentWeapon?.OnRightClickDown();
    }
    
    private void OnRightClickUp()
    {
        currentWeapon?.OnRightClickUp();
    }

    #endregion

    #region Weapon Equip
    public void EquipWeapon(WeaponData weapon)
    {
        if(currentWeaponData == weapon)
        {
            return;
        }
        currentWeaponData = weapon;
        if (currentWeapon)
        {
            currentWeapon.transform.DOLocalMove(hiddenPosition, animationDuration/2)
                     .SetEase(Ease.InBack)
                     .OnComplete(() =>
                     {
                         Destroy(currentWeapon.gameObject);
                         equipWeapon(weapon);
                     });
        }
        else
        {
            equipWeapon(weapon);
        }
    }
    
    private void UnequipWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.transform.DOLocalMove(hiddenPosition, animationDuration)
                     .SetEase(Ease.InBack)
                     .OnComplete(() =>
                     {
                         Destroy(currentWeapon.gameObject);
                         currentWeapon     = null;
                         currentWeaponData = null;
                     });
        }
    }
    
    private void equipWeapon(WeaponData weapon)
    {
        currentWeapon = Instantiate(weapon.weaponPrefab, transform)
            .GetComponent<WeaponBase>();
        currentWeapon.SetWeapon(playerController);
        currentWeapon.transform.localPosition = hiddenPosition;
        currentWeapon.transform.DOLocalMove(visiblePosition, animationDuration)
                     .SetEase(Ease.OutSine);
    }
    #endregion
    
    private void Special()
    {
        currentWeapon?.Special();
    }
    private void Attack()
    {
        currentWeapon?.TryAttack();
    }
}