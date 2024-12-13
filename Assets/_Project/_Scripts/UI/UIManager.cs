using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using _Project._Scripts.ScriptBases;
using _Project._Scripts.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Menu weaponWheelUI;
    [SerializeField] private Menu inventoryUI;
    [SerializeField] private Menu pauseMenuUI;
    
    private List<Menu> allMenus = new List<Menu>();
    private Menu currentOpenMenu;


    public static event Action<bool> OnMenuToggle;

    public bool IsAnyMenuOpen => currentOpenMenu != null;
    
    
    private void Awake()
    {
        
            if (Instance == null)
            {
                Instance = this;
            }
            else 
            {
                Destroy(gameObject);
            }
        
        SaveSystem.OnLoad += SetupUIs;
        SetupListeners();
    }


    private void SetupListeners()
    {
        InputManager.OpenInventoryMenu += HandleOpenInventoryMenu;
        InputManager.OpenPauseMenu += HandleOpenPauseMenu;
        InputManager.OpenInventoryEvent += HandleOpenWeaponWheel;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        
        InputManager.OpenInventoryMenu -= HandleOpenInventoryMenu;
        InputManager.OpenPauseMenu -= HandleOpenPauseMenu;
        InputManager.OpenInventoryEvent -= HandleOpenWeaponWheel;

        SaveSystem.OnLoad -= SetupUIs;
    }

    
    void SetupUIs()
    {
        SaveSystem.OnLoad -= SetupUIs;
        weaponWheelUI.SetupMenu();
        inventoryUI.SetupMenu();
        pauseMenuUI.SetupMenu();
        
        allMenus.Add(weaponWheelUI);
        allMenus.Add(inventoryUI);
        allMenus.Add(pauseMenuUI);
        
    }
    
    private void HandleOpenWeaponWheel()
    {
        ToggleMenu(weaponWheelUI);
    }

    private void HandleOpenInventoryMenu()
    {
        ToggleMenu(inventoryUI);
    }

    private void HandleOpenPauseMenu()
    {
        ToggleMenu(pauseMenuUI);
    }

    public void SetMenuOpened(Menu menu)
    {
        currentOpenMenu = menu;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        OnMenuToggle?.Invoke(true);
    }
    public void SetMenuClosed(Menu menu)
    {
        currentOpenMenu = null;
        if (!currentOpenMenu/*_openMenus.Count == 0*/)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            OnMenuToggle?.Invoke(false);
        }
    }

    public void ToggleMenu(Menu menu)
    {
        if(menu == null) return;
        if (currentOpenMenu != null && !currentOpenMenu.isActiveAndEnabled)
            currentOpenMenu = null;
    
        // If it is already open, close
        if (currentOpenMenu == menu)
        {
            currentOpenMenu.ToggleMenu();
            currentOpenMenu = null;
            OnMenuToggle?.Invoke(false);
            return;
        }

        // If another menu is already open close the previous one
        if (currentOpenMenu != null && currentOpenMenu != menu)
        {
            currentOpenMenu.ToggleMenu();
        }
        
        menu.ToggleMenu();
        currentOpenMenu = menu;
        OnMenuToggle?.Invoke(true);
    }
    
    public void CloseCurrentMenu()
    {
        if (currentOpenMenu != null)
        {
            currentOpenMenu.ToggleMenu();
            currentOpenMenu = null;
            OnMenuToggle?.Invoke(false);
        }
    }
    
}