using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;


[CreateAssetMenu(fileName = "InventoryDatabase", menuName = "Inventory/Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField, AssetList(Path = "_Project/Data/Items")]
    private List<ItemData> availableItems = new List<ItemData>();
    private Dictionary<string, ItemData> itemLookup = new Dictionary<string, ItemData>();

    //public static ItemDatabase Instance { get; private set; }
    private void Awake()
    {
        //SaveSystem.OnLoad += BuildLookupDictionary;
        BuildLookupDictionary();
    }
    private void OnEnable()
    {
        //BuildLookupDictionary();
    }

    public void BuildLookupDictionary()
    {
        itemLookup.Clear();
        foreach (var item in availableItems) 
        {
            if(!itemLookup.ContainsKey(item.itemID))
            {
                itemLookup.Add(item.itemID, item);
            }
            else
            {
                Debug.LogError($"Duplicate item ID found: {item.itemID}");
            }
        }
    }

    public ItemData GetItemData(string id) 
    {
        if(itemLookup.TryGetValue(id,out ItemData itemData))
        {
            return itemData;
        }
        else
        {
            Debug.LogError($"Item with ID {id} not found in database.");
            return null;
        }
    }
}
