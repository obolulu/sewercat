using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

namespace _Project._Scripts.PlayerScripts.Stats
{
    public class PlayerStatsHandler : MonoBehaviour
    {
        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] public ItemDatabase database;

        public static PlayerInventory playerInventory;
        public static PlayerStats playerStats;
        public static PlayerStatsHandler Instance;
        public static Action OnStatsInitialize;

        private void Awake()
        {
            SaveDirectory.SaveSystem.OnStartup += Initialize;
        }

        private void Initialize()
        {
            Instance        = this;
            playerStats     = new PlayerStats();
            playerInventory = new PlayerInventory();
            database.BuildLookupDictionary();
            if (SaveDirectory.SaveSystem.Instance != null)
            {
                SaveDirectory.SaveSystem.Instance.LoadGame();
            }
            else
            {
                Debug.LogError("SaveSystem instance is not initialized.");
            }
            OnStatsInitialize?.Invoke();;
        }

        private void OnDestroy()
        {
            SaveDirectory.SaveSystem.OnStartup -= Initialize;
        }

        public event Action OnHealthChanged;
        public event Action OnManaChanged;
        
        public void TakeDamage(float damage)
        {
            playerStats.Health -= damage;
            OnHealthChanged?.Invoke();
            if (playerStats.Health <= 0)
            {
                playerStats.Health = 0;
                //Die();
            }
        }
        
        /*
        public void Die()
        {
            dialogueRunner.StartDialogue("Death");
        }
        */
        public void Blocking()
        {
            
        }
        
        
        public void Heal(float amount)
        {
            playerStats.Health += amount;
            OnHealthChanged?.Invoke();
            if (playerStats.Health > playerStats.MaxHealth)
            {
                playerStats.Health = playerStats.MaxHealth;
            }
        }
        
        public void RestoreMana(float amount)
        {
            playerStats.Mana += amount;
            OnManaChanged?.Invoke();
            if (playerStats.Mana > playerStats.MaxMana)
            {
                playerStats.Mana = playerStats.MaxMana;
            }
        }
        
        public void SpendMana(float amount)
        {
            OnManaChanged?.Invoke();
            playerStats.Mana -= amount;
            if (playerStats.Mana < 0)
            {
                playerStats.Mana = 0;
            }
        }

        public void AddItem(ItemData itemData)
        {
            playerInventory.AddItem(itemData);
        }
        
        [YarnCommand("AddItem")]
        public static void AddItem(string itemID)
        {
            playerInventory.AddItem(Instance.database.GetItemData(itemID));
        }
        
        public void RemoveItemFromInventory(ItemData itemData)
        {
            playerInventory.RemoveItem(itemData);
        }

        public void AddInteractedItem(int item)
        {
            playerStats.InteractedItems.Add(item);
        }

        public void RemoveInteractedItem(int item)
        {
            playerStats.InteractedItems.Remove(item);
        }
        
        [YarnFunction("playerHasItem")]
        public static bool PlayerHasItem(string item)
        {
            Debug.Log("Checking for item: " + item);
            var inventory = playerInventory.GetInventory();
            for (int i = 0; i <inventory.Count; i++)
            {
                if (inventory[i].itemID == item)
                {
                    return true;
                }
            }

            return false;
        }
        
        /*
        public List<ItemData> GetInventoryItems()
        {
            List<ItemData> items = new List<ItemData>();
            foreach (var item in playerStats.InventoryDatas)
            {
                var toAdd = database.GetItemData(item);
                items.Add(toAdd);
            }
            return items;
        }
        */
        
        
        /*
         !!!!!! 
        public List<ItemData> FilterItemsByType(string typeName)
        {
            var items   = playerStats.InventoryDatas;
            var matches = items.Where(item => item.GetType().Name.Equals(typeName)).ToList();
            return matches;
        }
        */
        public void SetMaxHealth(float maxHealth)
        {
            playerStats.MaxHealth = maxHealth;
        }
        
        public void SetMaxMana(float maxMana)
        {
            playerStats.MaxMana = maxMana;
        }
        
        public void SetSpeed(float speed)
        {
            playerStats.Speed = speed;
        }

        public PlayerStats GetStats()
        {
            return playerStats;
        }
    }
}