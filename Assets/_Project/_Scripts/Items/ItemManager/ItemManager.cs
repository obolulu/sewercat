using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using _Project._Scripts.PlayerScripts.SaveSystem;
using _Project._Scripts.PlayerScripts.Stats;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public static GameObject  itemManager;
    public        int[]       items;

    private void Awake()
    {
        instance = this;
        itemManager = instance.gameObject;
        items   = new int[itemManager.transform.childCount];
        if (instance == null)
        {
            instance = this;
        }

        SaveSystem.OnSave += OnSaveGame;
        SaveSystem.OnLoad += OnLoadGame;
        InstantiateItems();
    }
    
    private void OnDestroy()
    {
        ItemManager.instance =  null;
        
        SaveSystem.OnSave -= OnSaveGame;
        SaveSystem.OnLoad -= OnLoadGame;
    }

    private void InstantiateItems()
    {
        for (int i = 0; i < itemManager.transform.childCount; i++)
        {
            items[i] = itemManager.transform.GetChild(i).gameObject.GetInstanceID();
        }
    }

    private void OnSaveGame()
    {
        
    }
    
private async void OnLoadGame()
{
    try
    {
        var data = await SaveSystem.Instance.LoadData();
        if (data == null || data.interactedItems == null) return;

        var interactedItems = data.interactedItems;
        
        for(int j = 0; j < items.Length; j++)
        {
            bool shouldBeActive = !interactedItems.Contains(items[j]);
            var item = itemManager.transform.GetChild(j).gameObject;
            
            if (item != null)
            {
                item.SetActive(shouldBeActive);
            }
        }
    }
    catch (Exception e)
    {
        Debug.LogError($"Failed to load items: {e.Message}");
    }
}
}