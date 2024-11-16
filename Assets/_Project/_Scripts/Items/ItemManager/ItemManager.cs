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
    
    private void OnLoadGame()
    {
        List<int> interactedItems = SaveSystem.Instance.LoadData().interactedItems;
        //shitty implementation, fix and make it better
            for(int j = 0; j < items.Length; j++)
            {
                if (interactedItems.Contains(items[j]))
                {
                    Destroy(itemManager.transform.GetChild(j).gameObject);
                }
            }
        
    }
}