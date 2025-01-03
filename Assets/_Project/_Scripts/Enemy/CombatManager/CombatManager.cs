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
    
        private readonly List<EnemyBase>    enemyList         = new();
        private          List<EnemyBase> aggressiveEnemies = new();
        private readonly List<EnemyBase> defensiveEnemies  = new();
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
            maxAggressors = 2;

            for (int i = aggressiveEnemies.Count - 1; i >= 0; i--)
            {
                var enemy = aggressiveEnemies[i];
                if (!enemy.WantsAggressive)
                {
                    enemy.SetStrategy(EnemyBase.EnemyStrategy.Defensive);
                    aggressiveEnemies.RemoveAt(i);
                }
            }

            var allPotentialAggressors = enemyList
                                         .Where(e => e.WantsAggressive)
                                         .OrderBy(e => e.DistanceToPlayer)
                                         .Take(maxAggressors)
                                         .ToList();

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

        
        private List<EnemyBase> GetNewCandidates()
        {
            return enemyList
                .Where(e => !aggressiveEnemies.Contains(e) && e.WantsAggressive)
                .OrderBy(e => e.DistanceToPlayer)
                .ToList();
        }
        
        private void CheckDefensiveEnemies()
        {
        
        }


        public void RegisterEnemy(EnemyBase enemy)
        {
            enemyList.Add(enemy);
        }
    
        public void UnregisterEnemy(EnemyBase enemy)
        {
            enemyList.Remove(enemy);
            aggressiveEnemies.Remove(enemy);
            defensiveEnemies.Remove(enemy);
        }

        public void MakeStrategyRequest(EnemyBase enemy)
        {
            
        }

        private void CheckAllEngagement()
        {
            List<EnemyBase> enemiesToDisengage = enemyList.Where(enemy => enemy.ShouldDisengage).ToList();
            
            foreach (var enemy in enemiesToDisengage)
            {
                enemy.Disengage();
            }
        }
    
    

    }
}