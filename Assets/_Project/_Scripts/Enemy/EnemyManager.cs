using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.Enemy;
using _Project._Scripts.PlayerScripts.SaveDirectory;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.EnemyDir
{
    public class EnemyManager : MonoBehaviour
    {
        public static            EnemyManager                                Instance;
        [SerializeField] private bool                                        debugMode;
        private                  Dictionary<string, EnemyBase>               enemyRegistry = new();
        private                  Dictionary<string ,EnemyBase.EnemySaveData> enemyDataMap  = new();
        private                  HashSet<EnemyBase>                          activeEnemies = new();

        private void CustomUpdate()
        {
            foreach (var enemy in activeEnemies)
            {
                if (enemy != null)
                {
                    enemy.CustomUpdate();
                }
            }
        }

        private IEnumerator PeriodicUpdate()
        {
            var wait = new WaitForSeconds(0.1f);
            while (true)
            {
                CustomUpdate();
                yield return wait;
            }

        }
        private void Awake()
        {
            //SaveSystem.OnSave += SaveEnemyData;
            SaveSystem.OnLoad += LoadEnemyData;
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        
        }

        private void Start()
        {
            RegisterEnemies();
            SetActiveEnemies();
            StartCoroutine(PeriodicUpdate());
        }
        
        private void SetActiveEnemies()
        {
            /*
            foreach(var enemy in enemyRegistry.Values)
            {
                if (enemy.isActiveAndEnabled)
                {
                    activeEnemies.Add(enemy);
                }
            }
            activeEnemies = enemyRegistry.Values.ToHashSet();
            */
            activeEnemies = enemyRegistry.Values
                                         .Where(enemy => enemy.isActiveAndEnabled)
                                         .ToHashSet();
        }

        private void RegisterEnemies()
        {
            enemyRegistry.Clear();
            foreach (Transform child in transform)
            {
                var enemy = child.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    string enemyId = enemy.Id;
                    enemyRegistry[enemyId] = enemy;
                    
                    EnemyBase.EnemySaveData initialData = enemy.GetSaveData();
                    enemyDataMap[enemyId] = initialData;

                    if (debugMode) Debug.Log($"Registered enemy: {enemyId}");
                }
            }
        }

        private void OnDestroy()
        {
            SaveSystem.OnLoad -= LoadEnemyData;
        }

        public void SetEnemyInactive(EnemyBase enemy)
        {
            if (activeEnemies.Contains(enemy))
            {
                activeEnemies.Remove(enemy);
            }
        }

        public EnemyDataCollection SaveEnemyData()
        {
            var collection = new EnemyDataCollection();
        
            foreach (var pair in enemyRegistry)
            {
                string    enemyId = pair.Key;
                EnemyBase enemy   = pair.Value;
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
                string    enemyId = kvp.Key;
                EnemyBase enemy   = kvp.Value;

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
            SetActiveEnemies();
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