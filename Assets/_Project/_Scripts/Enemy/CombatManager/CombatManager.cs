using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.PlayerScripts;
using UnityEngine;

namespace _Project._Scripts.Enemy.CombatManager
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] PlayerController player;
        public static CombatManager Instance;
    
        // DECLARING DIFFICULTY AS A RANDOM NUMBER HERE FOR NOW, CHANGE!!!
        private int difficulty = 1;
        private const int BaseAggressorCount = 1; // without difficulty
        private int maxAggressors;
    
        private readonly List<Enemy1> enemyList = new();
        private List<Enemy1> aggressiveEnemies = new();
        private readonly List<Enemy1> defensiveEnemies = new();
        //public fields
        public Vector3 PlayerPos => player.PlayerPosition;
        private void Awake()
        {
            if(CombatManager.Instance == null)
            {
                CombatManager.Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        

        }
    
        //is called in FixedUpdate for now, will change in the future to tick after a while like 0.2 or 0.5 seconds
        private void FixedUpdate()
        {
            CustomUpdate();
        }
    
        //will make decisions about the new combat goal (aggressive, defensive, flee etc.)
        private void CustomUpdate()
        {
            CheckAllEngagement();
            CheckAggressiveEnemies();
            CheckDefensiveEnemies();
        }

        private void CheckAggressiveEnemies()
        {
            maxAggressors = Mathf.Min(enemyList.Count,BaseAggressorCount + difficulty);
            var currentAggressors = aggressiveEnemies.Where(e => e.WantsAgressive).ToList();
            var newCandidates = GetNewCandidates();
            aggressiveEnemies = currentAggressors;
        
            foreach (var candidate in newCandidates)
            {
                if (aggressiveEnemies.Count >= maxAggressors) break;
                aggressiveEnemies.Add(candidate);
                defensiveEnemies.Remove(candidate);
            }
            return;
        
            List<Enemy1> GetNewCandidates()
            {
                return enemyList
                    .Where(e => !aggressiveEnemies.Contains(e) && e.WantsAgressive)
                    .OrderBy(e => e.DistanceToPlayer)
                    .ToList();
            }
        }

        private void CheckDefensiveEnemies()
        {
        
        }


        public void RegisterEnemy(Enemy1 enemy)
        {
            enemyList.Add(enemy);
        }
    
        public void UnregisterEnemy(Enemy1 enemy)
        {
            enemyList.Remove(enemy);
            aggressiveEnemies.Remove(enemy);
            defensiveEnemies.Remove(enemy);
        }

        private void CheckAllEngagement()
        {
            foreach (var enemy in enemyList)
            {
                CheckEngagement(enemy);
            }

            return;
        
            void CheckEngagement(Enemy1 enemy)
            {
                if(enemy.ShouldDisengage)
                {
                    enemy.Disengage();
                }
            }
        }
    
    

    }
}