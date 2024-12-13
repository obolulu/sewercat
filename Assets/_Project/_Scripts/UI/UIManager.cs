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
    

    private void Awake()
    {
        SetupListeners();
        SaveSystem.OnLoad += SetupUIs;
    }


    private void SetupListeners()
    {
        InputManager.OpenInventoryMenu += () => inventoryUI.ToggleMenu();
        InputManager.OpenPauseMenu += () => pauseMenuUI.ToggleMenu();
        InputManager.OpenInventoryEvent += () => weaponWheelUI.ToggleMenu();
    }

    private void OnDestroy()
    {
        InputManager.OpenInventoryMenu  -= () => inventoryUI.ToggleMenu();
        InputManager.OpenPauseMenu      -= () => pauseMenuUI.ToggleMenu();
        InputManager.OpenInventoryEvent += () => weaponWheelUI.ToggleMenu();

        SaveSystem.OnLoad -= SetupUIs;
    }

    
    void SetupUIs()
    {
        SaveSystem.OnLoad -= SetupUIs;
        weaponWheelUI.SetupMenu();
        inventoryUI.SetupMenu();
        pauseMenuUI.SetupMenu();
    }
}