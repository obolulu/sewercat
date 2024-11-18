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
        [SerializeField] private ItemDatabase database;

        public static PlayerStats playerStats;
        
        private void Awake()
        {
            playerStats = new PlayerStats();
            if (SaveDirectory.SaveSystem.Instance != null)
            {
                SaveDirectory.SaveSystem.Instance.LoadGame();
            }
            else
            {
                Debug.LogError("SaveSystem instance is not initialized.");
            }
            //dialogueRunner?.AddFunction("playerHasItem", new Func<string, object>((string item) => { return PlayerHasItem(item); }));
            dialogueRunner?.AddFunction("addItemToInventory", new Func<string, object>((string item) => { AddItemToInventory(item); return null; }));
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
        
        public void AddItemToInventory(string item)
        {
            playerStats.InventoryItems.Add(item);
        }
        
        public void RemoveItemFromInventory(string item)
        {
            playerStats.InventoryItems.Remove(item);
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
            return playerStats.InventoryItems.Contains(item);
        }
        
        public static List<string> GetInventoryItemsID()
        {
            return playerStats.InventoryItems;
        }

        public List<ItemData> GetInventoryItems()
        {
            List<ItemData> items = new List<ItemData>();
            foreach (var item in playerStats.InventoryItems)
            {
                var toAdd = database.GetItemData(item);
                items.Add(toAdd);
            }
            return items;
        }
        
        public List<ItemData> FilterItemsByType(string typeName)
        {
            var items   = GetInventoryItems();
            var matches = items.Where(item => item.GetType().Name.Equals(typeName)).ToList();
            return matches;
        }
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