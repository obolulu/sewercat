using System;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.Items;
using _Project._Scripts.PlayerScripts.SaveSystem;
using _Project._Scripts.PlayerScripts.Stats;
using Sirenix.OdinInspector;
using UnityEditor.Experimental.RestService;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerInventory

{

    //[ShowInInspector] private List<Item> items = new List<Item>();
    
    
    private List<List<ItemData>>             inventory      = new List<List<ItemData>>();
    
    private Dictionary<Type, List<ItemData>> _inventoryCategories;
    
    private int            inventoryCount = 4;

    public PlayerInventory()
    {
        InitializePlayerInventory();
    }
    public void InitializePlayerInventory()
    {
        Debug.Log("Initializing Player Inventory");
        _inventoryCategories ??= new Dictionary<Type, List<ItemData>>
        {
            { typeof(WeaponData), new List<ItemData>() },
            { typeof(QuestItemData), new List<ItemData>() },
            { typeof(ClothingData), new List<ItemData>() },
            { typeof(ItemData), new List<ItemData>() }
        };
        Debug.Log(_inventoryCategories);
    }

    public void LoadInventory(SaveData data)
    {
        foreach (var item in data.inventoryItems)
        {
            AddItem(item);
        }
    }

    public void AddItem(ItemData item)
    {
        Type itemType = item.GetType();
        _inventoryCategories[itemType].Add(item);
    }
    public void RemoveItem(ItemData item)
    {
        var itemType = item.GetType();
        _inventoryCategories[itemType].Remove(item);
    }
    
    List<ItemData> ReturnInventory(Type type)
    {
        return _inventoryCategories[type];
    }
    
    public void ClearInventory()
    {
        //_inventoryCategories.Clear();
        foreach (var category in _inventoryCategories)
        {
            category.Value.Clear();
        }
    }
    
    public void AddItem(string item)
    {
        var itemData = PlayerStatsHandler.Instance.database.GetItemData(item);
        AddItem(itemData);
    }
    public void RemoveItem(string item)
    {
        ItemData itemData = PlayerStatsHandler.Instance.database.GetItemData(item);
        RemoveItem(itemData);
    }
    
    public List<ItemData> GetInventory()
    {
        Debug.Log(_inventoryCategories.SelectMany(category => category.Value).ToList());
        return _inventoryCategories.SelectMany(category => category.Value).ToList();
    }
    
    public List<ItemData> GetInventory(Type type)
    {
        return _inventoryCategories[type];
    }
    /*
    private Type GetItemCategory(ItemData item)
    {
        if (item is WeaponItem)
            return typeof(WeaponItem);
        if (item is ClothingItem)
            return typeof(ClothingItem);
        if (item is QuestItem)
            return typeof(QuestItem);
        return typeof(ItemData);
    }
    */
}








/*
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
    */




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
    /*
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
*/