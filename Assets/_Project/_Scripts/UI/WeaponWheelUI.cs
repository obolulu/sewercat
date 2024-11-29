using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.Items;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponWheelUI : MonoBehaviour
{
    [System.Serializable]
    public class WeaponChoice
    {
        public string     name;
        public WeaponData weaponData;
        public Sprite icon;
    }
    
    [Header(("Wheel Settings"))]
    [SerializeField] private float wheelRadius = 150f;
    [SerializeField] private List<WeaponChoice> weapons       = new List<WeaponChoice>();
    [SerializeField] private float slowMotionTimeScale = 0.2f;
    
    [Header("References")] 
    [SerializeField] private Transform selectedWeaponSlot;
    [SerializeField]             private GameObject      wheelUI;
    [SerializeField]             private TextMeshProUGUI text;
    [SerializeField] private WeaponHandler               weaponHandler;
    
    [Header("UI References")]
    [SerializeField] private Transform wheelCenter;
    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private float      iconRadius = 120f;
    
    
    private List<GameObject> weaponIcons = new List<GameObject>();

    private bool isWheelActive = false;
    private int currentWeaponIndex;
    private GameObject currentWeapon;
    
    private List<ItemData> weaponItems = new List<ItemData>();
    
    private void Start()
    {
        wheelUI.SetActive(false);
        
        if(weapons.Count > 0)
        {
            EquipWeapon(0);
        }
        InputManager.OpenInventoryEvent += ToggleWheelOn;
        InputManager.CloseInventoryEvent += ToggleWheelOff;
        SaveSystem.OnLoad += InstantiateWeapons;
        InstantiateWeapons();
    }
    
    private void Update()
    {
        if (isWheelActive)
        {
            HandleWeaponSelection();
        }
    }
    
    private void HandleWeaponSelection()
    {
        // Get mouse position relative to wheel center
        Vector2 mousePos    = Input.mousePosition;
        Vector2 wheelCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 direction   = (mousePos - wheelCenter).normalized;

        // Calculate angle from mouse 
        float angle          = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        // Calculate which weapon slot the angle corresponds to
        int   segments       = weapons.Count;
        float segmentSize    = 360f                                  / segments;
        int   newWeaponIndex = Mathf.RoundToInt(angle / segmentSize) % segments;
        if (Vector2.Distance(mousePos, wheelCenter) > wheelRadius * 0.3f)
        {
            if (newWeaponIndex != currentWeaponIndex)
            {
                // Preview weapon change
                currentWeaponIndex = newWeaponIndex;
                // TODO: Play preview sound or show highlight
            }

            if (Input.GetMouseButtonDown(0))
            {
                EquipWeapon(currentWeaponIndex);
                isWheelActive = false;
                wheelUI.SetActive(false);
                Time.timeScale   = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible   = false;
            }
        }
    }
    
    private void EquipWeapon(int index)
    {
        // Destroy current weapon if it exists
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Spawn new weapon
        WeaponChoice weapon = weapons[index];
        //currentWeapon = Instantiate(weapon.weaponPrefab, selectedWeaponSlot.position, selectedWeaponSlot.rotation, selectedWeaponSlot);
        currentWeaponIndex = index;
        text.text = weapon.name;
        weaponHandler.EquipWeapon(weapon.weaponData);
        
    }
    
    
    
    private void InstantiateWeapons()
    {
        ClearWeapons();
        ClearIcons();
        
        List<ItemData> itemChoices = PlayerStatsHandler.playerInventory.GetInventory(typeof(WeaponData));
        foreach (var item in itemChoices)
        {
            WeaponData weaponData = (WeaponData) item;
            AddWeapon(weaponData);
        }
        PositionWeaponIcons();
    }

    private void PositionWeaponIcons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            float angle = (i * 2 * Mathf.PI) / weapons.Count;
            float x     = Mathf.Cos(angle)   * iconRadius;
            float y     = Mathf.Sin(angle)   * iconRadius;
        
            GameObject    icon          = Instantiate(iconPrefab, wheelCenter);
            RectTransform rectTransform = icon.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(x, y);
        
            // Set the icon sprite
            Image iconImage = icon.GetComponent<UnityEngine.UI.Image>();
            iconImage.sprite = weapons[i].icon;
        
            weaponIcons.Add(icon);
        }
    }

    private void ClearIcons()
    {
        foreach (var icon in weaponIcons)
        {
            if (icon != null)
                Destroy(icon);
        }
        weaponIcons.Clear();
    }
    
    
    private void AddWeapon(WeaponData weaponData)
    {
        WeaponChoice newWeapon = new WeaponChoice
        {
            name         = weaponData.itemName,
            weaponData   = weaponData,
            icon         = weaponData.itemIcon
        };
        weapons.Add(newWeapon);
    }

    private void RemoveWeapon(WeaponData weaponData)
    {
        WeaponChoice weaponToRemove = weapons.Find(w => w.name == weaponData.itemName);
        weapons.Remove(weaponToRemove);
    }
    
    private void ClearWeapons()
    {
        weapons.Clear();
    }
    
    private void ToggleWheelOn()
    {
        if (isWheelActive)
        {
            ToggleWheelOff();
            return;
        }
        isWheelActive = true;
        wheelUI.SetActive(true);
        Time.timeScale   = slowMotionTimeScale;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true; 
    }

    private void ToggleWheelOff()
    {
        isWheelActive = false;
        wheelUI.SetActive(false);
        Time.timeScale   = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }
    
    
}