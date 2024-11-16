using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField]             private ItemData _itemData;
    [SerializeField] private PlayerStatsHandler playerStatsHandler;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public string itemID;
    public int itemValue;
    
    private void Start()
    {
        if (playerStatsHandler == null)
        {
            Debug.LogError("PlayerStatsHandler not found in the scene.");
            return;
        }
        InitializeItem();
    }

    private void InitializeItem()
    {
        if (_itemData != null)
        {
            itemName        = _itemData.itemName;
            itemDescription = _itemData.itemDescription;
            itemIcon        = _itemData.itemIcon;
            itemID          = _itemData.itemID;
            itemValue       = _itemData.itemValue;
        }
        else
        {
            Debug.LogError("ItemData is not assigned.");
        }
    }
    
    
    public void onInteract()
    {
        if (playerStatsHandler)
        {
            playerStatsHandler.AddItemToInventory(itemID);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("PlayerStatsHandler is not assigned.");
        }
        MakeItemInteracted();
    }

    private void MakeItemInteracted()
    {
        var items = ItemManager.instance.items;
        int toAdd =  gameObject.GetInstanceID();
        PlayerStatsHandler.playerStats?.InteractedItems?.Add(toAdd);
    }
}