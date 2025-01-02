using System.Collections.Generic;
using System.Linq;
using _Project._Scripts.PlayerScripts;
using _Project._Scripts.ScriptBases;
using UnityEngine;

namespace _Project._Scripts.Enemy
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
/*
        private void CheckAggressiveEnemies()
        {
            maxAggressors = 2; // Could be calculated based on difficulty

            // First: Remove enemies that no longer want to be aggressive
            for (int i = aggressiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = aggressiveEnemies[i];
                if (!enemy.WantsAgressive)
                {
                    enemy.SetStrategy(EnemyBase.EnemyStrategy.Defensive);
                    aggressiveEnemies.RemoveAt(i);
                }
            }

            // Then: If we need more aggressors, get the closest eligible enemies
            if (aggressiveEnemies.Count < maxAggressors)
            {
                var availableSpots = maxAggressors - aggressiveEnemies.Count;
        
                var candidates = enemyList
                                 .Where(e => !aggressiveEnemies.Contains(e) && e.WantsAgressive)
                                 .OrderBy(e => e.DistanceToPlayer)
                                 .Take(availableSpots);

                foreach (var candidate in candidates)
                {
                    candidate.SetStrategy(EnemyBase.EnemyStrategy.Aggressive);
                    aggressiveEnemies.Add(candidate);
                }
            }
        }
        */
        private void CheckAggressiveEnemies()
        {
            maxAggressors = 2;

            // First: Remove enemies that no longer want to be aggressive
            for (int i = aggressiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = aggressiveEnemies[i];
                if (!enemy.WantsAgressive)
                {
                    enemy.SetStrategy(EnemyBase.EnemyStrategy.Defensive);
                    aggressiveEnemies.RemoveAt(i);
                }
            }

            // Get ALL potential aggressors sorted by distance
            var allPotentialAggressors = enemyList
                                         .Where(e => e.WantsAgressive)
                                         .OrderBy(e => e.DistanceToPlayer)
                                         .Take(maxAggressors)
                                         .ToList();

            // Remove current aggressors that aren't in the top closest enemies
            for (int i = aggressiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = aggressiveEnemies[i];
                if (!allPotentialAggressors.Contains(enemy))
                {
                    enemy.SetStrategy(EnemyBase.EnemyStrategy.Defensive);
                    aggressiveEnemies.RemoveAt(i);
                }
            }

            // Add new closest enemies that aren't already aggressive
            foreach (var candidate in allPotentialAggressors)
            {
                if (!aggressiveEnemies.Contains(candidate))
                {
                    candidate.SetStrategy(EnemyBase.EnemyStrategy.Aggressive);
                    aggressiveEnemies.Add(candidate);
                }
            }
        }

        
        private List<Enemy1> GetNewCandidates()
        {
            return enemyList
                .Where(e => !aggressiveEnemies.Contains(e) && e.WantsAgressive)
                .OrderBy(e => e.DistanceToPlayer)
                .ToList();
        }
        /*
        private void SetAggressiveEnemies()
        {
            foreach(var enemy in aggressiveEnemies)
            {
                enemy.Strategy = EnemyBase.EnemyStrategy.Aggressive;
            }
        }*/
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
            List<Enemy1> enemiesToDisengage = enemyList.Where(enemy => enemy.ShouldDisengage).ToList();
            
            foreach (var enemy in enemiesToDisengage)
            {
                enemy.Disengage();
            }
        }
    
    

    }
}