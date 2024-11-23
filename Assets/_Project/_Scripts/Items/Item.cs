using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField]             private ItemData _itemData;
    [SerializeField] private PlayerStatsHandler playerStatsHandler;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public string itemID;
    public int itemValue;
    
    public int quantity;

    private void Start()
    {
        if (playerStatsHandler == null)
        {
            Debug.LogError("PlayerStatsHandler not found in the scene.");
            return;
        }
        InitializeItem();
    }

    public void InitializeItem(ItemData itemdata = null)
    {
        if (itemdata == null)
        {
            itemdata = _itemData;
        }
    
        if (itemdata != null)
        {
            itemName        = itemdata.itemName;
            itemDescription = itemdata.itemDescription;
            itemIcon        = itemdata.itemIcon;
            itemID          = itemdata.itemID;
            itemValue       = itemdata.itemValue;
        }
        else
        {
            Debug.LogError("ItemData is not assigned.");
        }
    }
    
    
    public override void onInteract()
    {
        if (playerStatsHandler)
        {
            playerStatsHandler.AddItem(_itemData);
            gameObject.SetActive(false);
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