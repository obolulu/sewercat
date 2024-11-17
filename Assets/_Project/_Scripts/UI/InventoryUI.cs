using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine.UI;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using UnityEngine.UIElements;
using TMPro;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerStatsHandler playerStatsHandler;
    [SerializeField] private Transform itemListContainer; // Parent object for item entries
    [SerializeField] private GameObject itemEntryPrefab; // Prefab for each item entry
    [SerializeField] private ScrollRect scrollRect; // Reference to ScrollRect component
    
    private List<ItemData> items;
    private List<GameObject> spawnedEntries;

    private void Awake()
    {
        items = new List<ItemData>();
        spawnedEntries = new List<GameObject>();
        SaveSystem.OnLoad += StartInventoryScreen;
    }

    private void OnEnable()
    {
        StartInventoryScreen();
    }

    private void OnDisable()
    {
        ClearInventoryDisplay();
        items.Clear();
    }

    private void StartInventoryScreen()
    {
        items.Clear();
        ClearInventoryDisplay();
        items = playerStatsHandler.GetInventoryItems();
        DrawInventoryList();
    }

    private void ClearInventoryDisplay()
    {
        foreach (GameObject entry in spawnedEntries)
        {
            Destroy(entry);
        }
        spawnedEntries.Clear();
    }
    /*
    private void DrawInventoryList()
    {
        foreach (ItemData item in items)
        {
            GameObject entryObject = Instantiate(itemEntryPrefab, itemListContainer);
            spawnedEntries.Add(entryObject);

            // Get references to Text components in the prefab
            TextMeshProUGUI itemNameText = entryObject.GetComponentInChildren<TextMeshProUGUI>();
            if (itemNameText != null)
            {
                itemNameText.text = item.itemName;
            }

            // Add button component for item selection
            UnityEngine.UI.Button itemButton = entryObject.GetComponent<UnityEngine.UI.Button>();
            if (itemButton != null)
            {
                itemButton.onClick.AddListener(() => OnItemSelected(item));
            }
        }
    }
    */
    private void OnItemSelected(ItemData item)
    {
        // Handle item selection here
        Debug.Log($"Selected item: {item.itemName}");
        // You can add additional functionality like showing item details, use options, etc.
    }
}











/*
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private PlayerStatsHandler playerStatsHandler;
    private List<ItemData> items;
    private List<Sprite> sprites;

    private void Awake()
    {
        items = new List<ItemData>();
        sprites = new List<Sprite>();
        SaveSystem.OnLoad += StartInventoryScreen;
    }
    private void OnEnable()
    {
        StartInventoryScreen();
    }

    private void OnDisable()
    {
        items.Clear();
        sprites.Clear();
    }

    private void StartInventoryScreen()
    {
        items.Clear();
        sprites.Clear();
        items = playerStatsHandler.GetInventoryItems();
        items.ForEach(item => sprites.Add(item.itemIcon));
        //DrawScreen();
    }


    //formerly: OnGUI
    /*
    private void DrawScreen()
    {
        for (int i = 0; i < items.Count; i++)
        {
            GUI.DrawTexture(new Rect(10 + i * 50, 10, 50, 50), sprites[i].texture);
        }
    }
    

    //private void 

}
*/