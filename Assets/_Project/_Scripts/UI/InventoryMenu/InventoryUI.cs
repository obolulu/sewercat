using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using _Project._Scripts.Items;
using _Project._Scripts.PlayerScripts.Stats;
using _Project._Scripts.ScriptBases;
using _Project._Scripts.UI;
using UnityEngine.UI;

public class InventoryUI : Menu
{
    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Button closeButton;
    
    [Header("Category Buttons")]
    [SerializeField] private Button weaponsButton;
    [SerializeField] private Button questItemsButton;
    [SerializeField] private Button clothingButton;
    
    [Header("Selected Item Details")]
    [SerializeField] private Image selectedItemIcon;
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;
    [SerializeField] private TextMeshProUGUI selectedItemValue;
    [SerializeField] private GameObject detailsPanel;
    
    
    [Header("extra references")]
    //[SerializeField] private PlayerStatsHandler playerStatsHandler;


    private PlayerInventory inventory;
    private List<ItemSlot>  itemSlots = new List<ItemSlot>();
    private Type            currentCategory;
    private bool            _isActive;
    
    /*
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    */
    public override void SetupMenu()
    {
        CleanupSubscriptions();
        SetupButtons();
        HideDetailsPanel();
        //LoadAllItems(); // Default view
        //PlayerStatsHandler.OnStatsInitialize += LoadAllItems;
    }
    private void CleanupSubscriptions()
    {
        closeButton.onClick.RemoveAllListeners();
        weaponsButton.onClick.RemoveAllListeners();
        questItemsButton.onClick.RemoveAllListeners();
        clothingButton.onClick.RemoveAllListeners();
        InputManager.OpenInventoryMenu -= ToggleMenu;
    }

    private void SetupButtons()
    {
        closeButton.onClick.AddListener(() => inventoryPanel.SetActive(false));
        
        weaponsButton.onClick.AddListener(() => LoadCategory(typeof(WeaponData)));
        questItemsButton.onClick.AddListener(() => LoadCategory(typeof(QuestItemData)));
        clothingButton.onClick.AddListener(() => LoadCategory(typeof(ClothingData)));
        //InputManager.OpenInventoryMenu += ToggleMenu;
    }

    private void OnDestroy()
    {
        //InputManager.OpenInventoryMenu       -= ToggleMenu;
        //PlayerStatsHandler.OnStatsInitialize -= LoadAllItems;
    }

    public override void ToggleMenu()
    {
        _isActive = !_isActive;
        if (_isActive)
            OpenInventory();
        else
            CloseInventory();
        RefreshCurrentView();
    }

    private void CloseInventory()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OpenInventory()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LoadCategory(Type categoryType)
    {
        inventory = PlayerStatsHandler.playerInventory;
        currentCategory = categoryType;
        ClearItems();
        
        var items = inventory.GetInventory(categoryType);
        foreach (var item in items)
        {
            CreateItemSlot(item);
        }
    }

    private void LoadAllItems()
    {
        inventory = PlayerStatsHandler.playerInventory;
        currentCategory = null;
        ClearItems();
        
        var items = inventory.GetInventory();
        foreach (var item in items)
        {
            CreateItemSlot(item);
        }
    }

    private void CreateItemSlot(ItemData item)
    {
        GameObject slotObject = Instantiate(itemSlotPrefab, itemsContainer);
        ItemSlot slot = slotObject.GetComponent<ItemSlot>();
        
        slot.Initialize(item);
        slot.OnSlotClicked += ShowItemDetails;
        itemSlots.Add(slot);
    }

    private void ShowItemDetails(ItemData item)
    {
        selectedItemIcon.sprite = item.itemIcon;
        selectedItemName.text = item.itemName;
        selectedItemDescription.text = item.itemDescription;
        selectedItemValue.text = $"Value: {item.itemValue}";
        detailsPanel.SetActive(true);
    }

    private void HideDetailsPanel()
    {
        detailsPanel.SetActive(false);
    }

    private void ClearItems()
    {
        foreach (var slot in itemSlots)
        {
            //Destroy(slot.gameObject);
            slot.gameObject.SetActive(false); // Set the slot to not show, but not destroy
        }
        itemSlots.Clear();
        HideDetailsPanel();
    }

    private void RefreshCurrentView()
    {
        if (currentCategory != null)
            LoadCategory(currentCategory);
        else
            LoadAllItems();
    }

    private void OnEnable()
    {
        if (inventory != null)
            RefreshCurrentView();
    }
}