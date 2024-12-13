using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using _Project._Scripts.ScriptBases;
using _Project._Scripts.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Menu weaponWheelUI;
    [SerializeField] private Menu inventoryUI;
    [SerializeField] private Menu pauseMenuUI;
    
    private List<Menu> allMenus = new List<Menu>();
    private Menu currentOpenMenu;


    public static event Action<bool> OnMenuToggle;

    public bool IsAnyMenuOpen => currentOpenMenu != null;
    
    
    private void Awake()
    {
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

    private void ToggleMenu(Menu menu)
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
    
}