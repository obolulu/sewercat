using System;
using _Project._Scripts.PlayerScripts.SaveSystem;
using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;
using UnityEngine.Windows;
using Vector3 = System.Numerics.Vector3;

namespace _Project._Scripts.PlayerScripts.SaveDirectory
{
    public class SaveSystem : MonoBehaviour
    {
        [SerializeField] private GameObject         player;
        [SerializeField] private PlayerStatsHandler playerStatsHandler;

        public static SaveSystem  Instance;
        
        private       string      _savePath;
        private       StatsBase[] stats;
        private       PlayerStats playerStats;

        public static Action OnSave;
        public static Action OnLoad;

        void Awake()
        {
            _savePath = Application.persistentDataPath + "/saveData.json";
            Instance              =  this;
            playerStats           =  playerStatsHandler.getStats() as PlayerStats;
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
                inventoryItems  = playerStats.InventoryItems,
                interactedItems = playerStats.InteractedItems,
                playerLocation = new UnityEngine.Vector3(player.transform.position.x, player.transform.position.y,
                    player.transform.position.z),
            };
            SaveData(data);
            OnSave?.Invoke();
        }

        public void LoadGame()
        {
            var data = LoadData();
            playerStats.Health         = data.playerHealth;
            playerStats.MaxHealth      = data.playerMaxHealth;
            playerStats.Mana           = data.playerMana;
            playerStats.MaxMana        = data.playerMaxMana;
            playerStats.InventoryItems = data.inventoryItems;
            playerStats.InteractedItems = data.interactedItems;
            
            player.transform.position =
                new UnityEngine.Vector3(data.playerLocation.x, data.playerLocation.y, data.playerLocation.z);
            
            OnLoad?.Invoke();

        }


    }


}