using _Project._Scripts.PlayerScripts.Stats;
using UnityEngine;
using UnityEngine.Windows;

namespace _Project._Scripts.PlayerScripts.SaveSystem
{
    public class SaveSystem : MonoBehaviour
    {
        private string _savePath;
        public static SaveSystem Instance;
        [SerializeField] private GameObject player;
        [SerializeField] private PlayerStats playerStats;
        
        
        
        void Start()
        {
            Instance = this;
            _savePath = Application.persistentDataPath + "/saveData.json";
        }
        
        private void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            System.IO.File.WriteAllText(_savePath, json);
            Debug.Log("Game Saved on: " + Application.persistentDataPath);
        }

        public SaveData LoadGame()
        {
            if(File.Exists(_savePath))
            {
                string json = System.IO.File.ReadAllText(_savePath);
                return JsonUtility.FromJson<SaveData>(json);
            }
            {
                Debug.LogError("Save file not found in " + _savePath);
                return null;
            }
        }

        public void SaveData(int cpID)
        {
            var data = new SaveData
            {
                version = "0.1",
                playerHealth = playerStats.Health,
                playerMana = playerStats.Mana,
                inventoryItems = playerStats.InventoryItems,
                playerCheckpoint = cpID
            };
            Save(data);
        }
        
        
    }
    

}