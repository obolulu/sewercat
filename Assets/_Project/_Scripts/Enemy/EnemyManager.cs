using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project._Scripts.EnemyDir
{
    public class EnemyManager : MonoBehaviour
    {
        List<EnemySaveData>                enemyDataList    = new List<EnemySaveData>();
        private List<Enemy>                enemies          = new List<Enemy>();
        private Dictionary<Enemy,EnemySaveData> enemyDataMap = new Dictionary<Enemy, EnemySaveData>();

        private void Start()
        {
            foreach (Transform child in transform)
            {
                var enemy = child.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemies.Add(enemy);
                }
            }        
        }

        public List<EnemySaveData> SaveEnemyData()
        {
            foreach (var enemy in enemies)
            {
                EnemySaveData data = enemy.GetSaveData();
                enemyDataList.Add(data);
            }
            return enemyDataList;
        }

    }
}