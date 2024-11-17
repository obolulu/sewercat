using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.Stats;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Required]
    [SerializeField] private ItemDatabase database;
    
    [ShowInInspector]
    private List<Item> items = new List<Item>();

    
    public void InitializeFromPlayerData(PlayerStats playerStats)
    {
        items.Clear();

        foreach (var savedItem in playerStats.InventoryItems)
        {
            var itemData = database.GetItemData(savedItem);
            if (itemData != null)
            {
                //var item = new Item(itemData);
            }
        }
    }
    




    /*
    public void AddItem(string itemId, int quantity = 1) 
    {
        var existingItem = items.Find(item => itemId == "itemID");
        if(existingItem) 
        {
            existingItem.quantity += quantity;
        }
        else
        {
            items.Add(new ItemData(database.GetItemData("itemID")));
        }
    }
    */
    /*
    public void RemoveItem(string itemId, int quantity = 1)
    {
        var existingItem = items.Find(item => item.itemID == itemId);
        if (existingItem != null)
        {
            existingItem.quantity -= quantity;
            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem);
            }
        }
    }
    */
    public ItemData GetItemData(string itemId)
    {
        return database.GetItemData(itemId);
    }

    public List<ItemData> GetAllItemsData()
    {
        List<ItemData> itemsData = new List<ItemData>();
        foreach (var item in items)
        {
            var data = GetItemData(item.itemID);
            if (data != null)
            {
                itemsData.Add(data);
            }
        }
        return itemsData;
    }

    public List<Item> GetAllItems()
    {
        List<Item> itemsList = new List<Item>();
        foreach (var item in items)
        {
            itemsList.Add(item);
        }
        return itemsList;
    }
}