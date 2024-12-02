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
    }
    
    void SetupUIs()
    {
        foreach (GameObject ui in _uiList)
        {
            //_currentInteractable = ui.GetComponent<IInteractable>();
            ui.GetComponent<IToggleMenu>().SetupMenu();
        }
    }
}