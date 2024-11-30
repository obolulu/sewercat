using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.ScriptBases;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private IToggleMenu dialogueUI;
    [SerializeField] private IToggleMenu inventoryUI;
    [SerializeField] private IToggleMenu pauseMenuUI;
    [SerializeField] private IToggleMenu playerUI;

    [SerializeField] private GameObject[] _uiList;
    private IInteractable _currentInteractable;

    private void Awake()
    {
        SetupUIs();
        InputManager.OpenInventoryEvent += ToggleInventoryUI;
        InputManager.OpenPauseMenu += TogglePauseMenu;
    }
    
    void SetupUIs()
    {
        foreach (GameObject ui in _uiList)
        {
            //_currentInteractable = ui.GetComponent<IInteractable>();
            ui.GetComponent<IToggleMenu>().SetupMenu();
        }
    }
    
    private void OnDestroy()
    {
        InputManager.OpenInventoryEvent -= ToggleInventoryUI;
        InputManager.OpenPauseMenu -= TogglePauseMenu;
    }

    private void ToggleInventoryUI()
    {
        inventoryUI.ToggleMenu();
    }

    private void TogglePauseMenu()
    {
        pauseMenuUI.ToggleMenu();
    }
}