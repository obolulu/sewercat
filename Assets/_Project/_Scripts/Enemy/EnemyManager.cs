using System;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using UnityEngine;

namespace _Project._Scripts.EnemyDir
{
    public class EnemyManager : MonoBehaviour
    {
        //List<EnemySaveData>                enemyDataList    = new List<EnemySaveData>();
        //private List<Enemy>                enemies          = new List<Enemy>();
        [SerializeField] private bool debugMode;
        private Dictionary<string, Enemy> enemyRegistry = new Dictionary<string, Enemy>();
        private Dictionary<string ,EnemySaveData> enemyDataMap = new Dictionary<string, EnemySaveData>();

        private void Awake()
        {
            //SaveSystem.OnSave += SaveEnemyData;
            SaveSystem.OnLoad += LoadEnemyData;
        
        }

        private void Start()
        {
            RegisterEnemies();
        }

        private void RegisterEnemies()
        {
            enemyRegistry.Clear();
            foreach (Transform child in transform)
            {
                var enemy = child.GetComponent<Enemy>();
                if (enemy != null)
                {
                    string enemyId = enemy.EnemyId;
                    enemyRegistry[enemyId] = enemy;
                    
                    EnemySaveData initialData = enemy.GetSaveData();
                    enemyDataMap[enemyId] = initialData;

                    if (debugMode) Debug.Log($"Registered enemy: {enemyId}");
                }
            }
        }

        private void OnDestroy()
        {
            SaveSystem.OnLoad -= LoadEnemyData;
        }

        public EnemyDataCollection SaveEnemyData()
        {
            var collection = new EnemyDataCollection();
        
            foreach (var pair in enemyRegistry)
            {
                string enemyId = pair.Key;
                Enemy  enemy   = pair.Value;
                if (enemy != null)
                {
                    var saveData = enemy.GetSaveData();
                    collection.enemies.Add(saveData);
                    enemyDataMap[enemyId] = saveData;
                    if (debugMode) Debug.Log($"Saved data for enemy: {enemyId}");
                }
                else
                {
                    if (debugMode) Debug.LogWarning($"Failed to save data for enemy: {enemyId} - Enemy reference is null");
                }
            }
        
            return collection;
        }

        
 
        public void LoadEnemyData()
        {
            if (enemyDataMap == null) return;
            
            foreach (var kvp in enemyRegistry)
            {
                string enemyId = kvp.Key;
                Enemy enemy = kvp.Value;

                if (enemy != null && enemyDataMap.ContainsKey(enemyId))
                {
                    enemy.LoadSaveData(enemyDataMap[enemyId]);
                    if (debugMode) Debug.Log($"Loaded data for enemy: {enemyId}");
                }
                else
                {
                    if (debugMode) Debug.LogWarning($"Failed to load data for enemy: {enemyId}");
                }
            }
        }
        public void UpdateSaveData(EnemyDataCollection collection)
        {
            if (collection == null || collection.enemies == null) return;
        
            enemyDataMap.Clear();
            foreach (var enemyData in collection.enemies)
            {
                if (enemyData != null && !string.IsNullOrEmpty(enemyData.id))
                {
                    enemyDataMap[enemyData.id] = enemyData;
                }
            }
        }
        /*
          public void SaveEnemyData()
        {
            foreach (var kvp in enemyRegistry)
            {
                string enemyId = kvp.Key;
                Enemy enemy = kvp.Value;

                if (enemy != null)
                {
                    enemyDataMap[enemyId] = enemy.GetSaveData();
                    if (debugMode) Debug.Log($"Saved data for enemy: {enemyId}");
                }
                else
                {
                    if (debugMode) Debug.LogWarning($"Failed to save data for enemy: {enemyId} - Enemy reference is null");
                }
            }
        }
         */
    }
    
}