using System;
using _Project._Scripts.Enemy;
using _Project._Scripts.EnemyDir;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.ScriptBases
{
    public abstract class EnemyBase : MonoBehaviour
    {
        #region ID

        [SerializeField] private string id; // Serialized for debugging
        public string Id
        {
            get
            {
                // Generate a new ID if none exists
                if (string.IsNullOrEmpty(id))
                {
                    id = System.Guid.NewGuid().ToString();
                }
                return id;
            }
            private set => id = value;
        }
        [Button("Generate New GUID")]
        private void GenerateNewGuid()
        {
            if(string.IsNullOrEmpty(id) ) Id = System.Guid.NewGuid().ToString();
            Debug.Log($"New GUID generated: {Id}");
        }

        #endregion
        
        [System.Serializable]
        public class EnemySaveData
        {
            public string id;
            public Vector3 position;
            public Quaternion rotation;
            public float health;
            public bool isDisengaged;
            public bool isInActiveCombat;
        }
        public enum EnemyStrategy
        {
            Aggressive,
            Defensive,
            Fleeing,
            Passive,
        }

        #region References

        [Header("References")]
        
        [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
        [SerializeField] protected BehaviourTreeOwner tree;
        //For the enemy barks with floating text
        [SerializeField] private FloatingText dialogue;
        //for checking if the player is in view
        [SerializeField] protected EnemyView enemyView;
        #endregion


        protected EnemyStrategy currentStrategy;
        
        public EnemyDefaultData enemyData;

        #region DistanceToPlayer

        private       float _cachedDistanceToPlayer;
        private       float _lastDistanceUpdateTime;
        private const float DistanceUpdateInterval = 0.1f;
        public float DistanceToPlayer {
            get {
                if (!(Time.time - _lastDistanceUpdateTime > DistanceUpdateInterval)) return _cachedDistanceToPlayer;
                _cachedDistanceToPlayer = Vector3.Distance(transform.position, CombatManager.Instance.PlayerPos);
                _lastDistanceUpdateTime = Time.time;
                return _cachedDistanceToPlayer;
            }
        }

        #endregion

        // Abstract properties and methods
        public abstract bool WantsAggressive { get; } // Changed to be abstract since Enemy1 overrides it
        //public abstract void CustomUpdate();

        private bool _isInActiveCombat = false;
        public  bool IsInCombat      => _isInActiveCombat;

        

        // Public property to access strategy
        public EnemyStrategy Strategy => currentStrategy;
        
        public bool ShouldDisengage => DistanceToPlayer >= enemyData.disengageRange;
        
        public bool IsLowOnHealth   => currentHealth <= enemyData.maxHealth * 0.2f;
        
        public float currentHealth;
        
        
        protected bool _isStunned;
        protected float _stunDuration;
        protected bool _isDisengaged = true;
        protected float _lastAttackTime;

        #region Update
        private void HandleStunState()
        {
            if (!_isStunned) return;

            _stunDuration -= Time.deltaTime;
            if (_stunDuration <= 0)
            {
                _isStunned = false;
                tree.blackboard.SetVariableValue("IsStunned", false);
            }
        }

        #endregion
        
        #region Initialization

        private void InitializeEnemy()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }

            currentHealth = enemyData.maxHealth;
            agent.speed = enemyData.moveSpeed;
            agent.angularSpeed = enemyData.rotationSpeed;
            InitializeViewColliders();
        }

        private void InitializeViewColliders()
        {
            enemyView.OnEngage += Engage;
        }

        #endregion

        #region strategy

        protected virtual void SetStrategy(EnemyStrategy newStrategy)
        {
            currentStrategy = newStrategy;
            if (agent != null)
            {
                agent.ResetPath();
            }
        }

        public virtual void ChangeStrategy(EnemyStrategy newStrategy)
        {
            if (currentStrategy == newStrategy) return;
            SetStrategy(newStrategy);
        }

        #endregion
        
        #region Save/Load

        public EnemySaveData GetSaveData()
        {
            return new EnemySaveData
            {
                id               = Id,
                position         = transform.position,
                rotation         = transform.rotation,
                health           = currentHealth,
                isDisengaged     = _isDisengaged,
                isInActiveCombat = _isInActiveCombat
            };
        }

        public void LoadSaveData(EnemySaveData data)
        {
            transform.position = data.position;
            transform.rotation = data.rotation;
            currentHealth = data.health;
            _isDisengaged = data.isDisengaged;
            _isInActiveCombat = data.isInActiveCombat;

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
            else
            {
                gameObject.SetActive(true);
            }

            SetupBlackboard();
        }

        
        #endregion

        #region combat functions

        public virtual void TakeDamage(float damage, Vector3 hitDirection)
        {
            float modifiedDamage = damage / enemyData.stunResistance;
            currentHealth = Mathf.Max(0, currentHealth - modifiedDamage);
            tree.blackboard.SetVariableValue("Health", currentHealth);

            if (dialogue != null)
            {
                dialogue.ShowDialogue("Ouch!");
                dialogue.HideDialogue();
            }

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }
        
        #region Engagement
        
        public void Engage()
        {
            _isInActiveCombat = true;
            CombatManager.Instance.RegisterEnemy(this);
            enemyView.DisableColliders();
            
        }
        
        public void Disengage()
        {
            _isInActiveCombat = false;
            CombatManager.Instance.UnregisterEnemy(this);
            enemyView.EnableColliders();
        }

        #endregion
        
        #endregion

        #region Unity Functions

        private void Awake()
        {
            InitializeEnemy();
        }

        private void OnEnable()
        {
            SetupBlackboard();
        }

        public virtual void CustomUpdate()
        {
            UpdateBlackboard();
        }
        #endregion
        
        # region Blackboard

        

        private void SetupBlackboard()
        {
            var blackboard = tree.blackboard;
            blackboard.SetVariableValue("MaxHealth", enemyData.maxHealth);
            blackboard.SetVariableValue("IsStunned", _isStunned);
            blackboard.SetVariableValue("IsDisengaged", _isDisengaged);
            blackboard.SetVariableValue("Agent", agent);
            blackboard.SetVariableValue("AttackRange", enemyData.attackRange);
            blackboard.SetVariableValue("EngageRange", enemyData.engageRange);
            blackboard.SetVariableValue("DisengageRange", enemyData.disengageRange);
            blackboard.SetVariableValue("AttackDamage", enemyData.attackDamage);
            blackboard.SetVariableValue("PatrolWaitTime", enemyData.patrolWaitTime);
        }

        private void UpdateBlackboard()
        {
            var blackboard = tree.blackboard;
            tree.Tick();
        }
        #endregion
    }
    
}