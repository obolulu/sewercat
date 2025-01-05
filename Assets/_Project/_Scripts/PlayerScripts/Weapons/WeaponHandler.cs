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
    
    [Header("Animation Settings")]
    [SerializeField] private Vector3 hiddenPosition = new Vector3(0, -0.5f, 0);
    [SerializeField] private Vector3 visiblePosition = new Vector3(0, 0.5f, 0);
    [SerializeField] private float animationDuration = 0.5f;    

    private void Awake()
    {
        InputManager.LeftClickDown += Attack;
        InputManager.RightClickDown += OnRightClickDown;
        InputManager.RightClickUp += OnRightClickUp;
        InputManager.Special += Special;
        InputManager.PutWeaponDown += UnequipWeapon;
    }

    private void OnDestroy()
    {
        InputManager.LeftClickDown -= Attack;
        InputManager.RightClickDown -= OnRightClickDown;
        InputManager.RightClickUp -= OnRightClickUp;
        InputManager.Special -= Special;
        InputManager.PutWeaponDown -= UnequipWeapon;
    }

    public void EquipWeapon(WeaponData weapon)
    {
        if(currentWeaponData == weapon) return;
        
        currentWeaponData = weapon;
        if (currentWeapon)
        {
            currentWeapon.transform.DOLocalMove(hiddenPosition, animationDuration/2)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    currentWeapon.ResetWeapon();
                    Destroy(currentWeapon.gameObject);
                    equipWeapon(weapon);
                });
        }
        else
        {
            equipWeapon(weapon);
        }
    }

    private void equipWeapon(WeaponData weapon)
    {
        // Instantiate the weapon
        var weaponObj = Instantiate(weapon.weaponPrefab, transform);
        currentWeapon = weaponObj.GetComponent<WeaponBase>();
        
        // Setup weapon
        currentWeapon.SetWeapon(playerController);
        currentWeapon.transform.localPosition = hiddenPosition;
        
        // Animate weapon into position
        currentWeapon.transform.DOLocalMove(visiblePosition, animationDuration)
            .SetEase(Ease.OutSine);
    }

    private void UnequipWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.ResetWeapon();
            currentWeapon.transform.DOLocalMove(hiddenPosition, animationDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    Destroy(currentWeapon.gameObject);
                    currentWeapon = null;
                    currentWeaponData = null;
                });
        }
    }

    private void Attack() => currentWeapon?.TryAttack();
    private void Special() => currentWeapon?.Special();
    private void OnRightClickDown() => currentWeapon?.OnRightClickDown();
    private void OnRightClickUp() => currentWeapon?.OnRightClickUp();
}