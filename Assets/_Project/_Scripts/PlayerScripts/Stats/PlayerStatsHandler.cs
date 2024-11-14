using System;
using UnityEngine;

namespace _Project._Scripts.PlayerScripts.Stats
{
    public class PlayerStatsHandler : MonoBehaviour
    {
        private PlayerStats playerStats;
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

        public PlayerStats getStats()
        {
            return playerStats;
        }
    }
}