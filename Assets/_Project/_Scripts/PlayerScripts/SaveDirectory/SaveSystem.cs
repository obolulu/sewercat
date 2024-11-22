using System;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.PlayerScripts.SaveSystem;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;
using UnityEngine.Windows;
using Vector3 = System.Numerics.Vector3;

namespace _Project._Scripts.PlayerScripts.SaveDirectory
{
    public class SaveSystem : MonoBehaviour
    {
        [SerializeField]            private GameObject         player;
        [SerializeField]            private PlayerStatsHandler playerStatsHandler;
        //[SerializeField] private ItemDatabase       database;
        public static                       SaveSystem         Instance;
        
        private       string      _savePath;
        private       StatsBase[] stats;
        private       PlayerStats playerStats;

        public static Action OnSave;
        public static Action OnLoad;

        void Awake()
        {
            _savePath = Application.persistentDataPath + "/saveData.json";
            Instance              =  this;
            playerStats           =  playerStatsHandler.GetStats() as PlayerStats;
            InputManager.SaveGame += SaveGame;
            InputManager.LoadGame += LoadGame;
        }

        private void OnDestroy()
        {
            InputManager.SaveGame -= SaveGame;
            InputManager.LoadGame -= LoadGame;
        }

        private void SaveData(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(_savePath, json);
            Debug.Log("Game Saved on: " + Application.persistentDataPath);
        }

        public SaveData LoadData()
        {
            if (File.Exists(_savePath))
            {
                string json = System.IO.File.ReadAllText(_savePath);
                return JsonUtility.FromJson<SaveData>(json);
            }

            {
                Debug.LogError("Save file not found in " + _savePath);
                return null;
            }
        }

        //turn to private after deciding what to do with checkpoints
        public void SaveGame()
        {
            var data = new SaveData
            {
                version         = "0.1",
                playerHealth    = playerStats.Health,
                playerMaxHealth = playerStats.MaxHealth,
                playerMana      = playerStats.Mana,
                playerMaxMana   = playerStats.MaxMana,
                inventoryItems = SaveItems(),
                interactedItems = playerStats.InteractedItems,
                playerLocation = new UnityEngine.Vector3(player.transform.position.x, player.transform.position.y,
                    player.transform.position.z),
            };
            //SaveItems(data);
            SaveData(data);
            OnSave?.Invoke();
        }

        public static List<string> SaveItems()
        {
            var inventory = PlayerStatsHandler.playerInventory.GetInventory();
            string[] itemIDs = new string[inventory.Count];
            for (int i = 0; i < inventory.Count; i++)
            {
                //data.inventoryItems.Add(playerStats.InventoryDatas[i].itemID);
                itemIDs[i] = (inventory[i].itemID);
            }

            return itemIDs.ToList();
        }

        private static void LoadItems(SaveData data)
        {
            PlayerStatsHandler.playerInventory.ClearInventory();
            for (var i = 0; i < data.inventoryItems.Count; i++)
            {
               PlayerStatsHandler.playerInventory.AddItem(data.inventoryItems[i]);
            }

        }
    
        public void LoadGame()
        {
            var data = LoadData();
            playerStats.Health          = data.playerHealth;
            playerStats.MaxHealth       = data.playerMaxHealth;
            playerStats.Mana            = data.playerMana;
            playerStats.MaxMana         = data.playerMaxMana;
            playerStats.InteractedItems = data.interactedItems;
            LoadItems(data);
            player.transform.position =
                new UnityEngine.Vector3(data.playerLocation.x, data.playerLocation.y, data.playerLocation.z);
            
            OnLoad?.Invoke();

        }


    }


}