using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.EnemyDir;
using _Project._Scripts.ScriptBases;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace _Project._Scripts.Enemy
{
    public class Enemy1 : EnemyBase, ICustomUpdate, IDamageable
    {
        [Serializable]
        public class EnemySaveData
        {
            public string id;
            public Vector3 position;
            public Quaternion rotation;
            public float health;
            public bool isDisengaged;
            public bool isInActiveCombat;
        }

        public EnemyDefaultData enemyData;

        #region Serialized Fields

        [Header("Enemy id")] [SerializeField] private string enemyId = Guid.NewGuid().ToString();

        [Header("Patrol Settings")] [SerializeField]
        List<GameObject> waypoints;

        //[SerializeField]  protected NavMeshAgent  agent;

        [Header("StateMachine")] [SerializeField]
        private EnemyStateMachine stateMachine;

        [SerializeField] private FloatingText dialogue;
        [SerializeField] private BehaviourTreeOwner tree;
        
        //for checking if the player is in view
        [SerializeField] private EnemyView enemyView;
        
        
        
        #endregion

        #region State Variables

        public float currentHealth;
        private bool _isStunned;
        private float _stunDuration;
        private bool _isInActiveCombat;
        private bool _isDisengaged = true;
        private float _lastAttackTime;


        #endregion

        #region Properties

        public string Id => enemyId;

        //used in the behaviour tree
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
        public override bool IsInCombat      => _isInActiveCombat;
        //public override bool WantsAggressive { get; }
        public          bool IsLowOnHealth   => currentHealth <= enemyData.maxHealth * 0.2f;


        //used in attack logic
        public bool CanAttack => Time.time - _lastAttackTime >= enemyData.attackCooldown;
        
        public bool ShouldDisengage => DistanceToPlayer >= enemyData.disengageRange;
        



        //bad naming, i know, but it is used in the combat manager to determine if the enemy should engage
        public override bool WantsAggressive
        {
            get
            {
                if (IsLowOnHealth) return false;
                //if (DistanceToPlayer > enemyData.engageRange) return false;
                return true;
            }
        }

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

        public override void CustomUpdate()
        {
            UpdateBlackboard();
            HandleStunState();
        }



        #endregion

        # region Setup

        private void InitializeEnemy()
        {
            if (string.IsNullOrEmpty(enemyId))
            {
                enemyId = Guid.NewGuid().ToString();
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

        private void SetupBlackboard()
        {
            var blackboard = tree.blackboard;
            //blackboard.SetVariableValue("Health", _currentHealth);
            blackboard.SetVariableValue("MaxHealth", enemyData.maxHealth);
            blackboard.SetVariableValue("IsStunned", _isStunned);
            //blackboard.SetVariableValue("IsInCombat", _isInActiveCombat);
            blackboard.SetVariableValue("IsDisengaged", _isDisengaged);
            //blackboard.SetVariableValue("PlayerTransform", player);
            //blackboard.SetVariableValue("Waypoints", waypoints);
            blackboard.SetVariableValue("Agent", agent);
            blackboard.SetVariableValue("AttackRange", enemyData.attackRange);
            blackboard.SetVariableValue("EngageRange", enemyData.engageRange);
            blackboard.SetVariableValue("DisengageRange", enemyData.disengageRange);
            blackboard.SetVariableValue("CanAttack", CanAttack);
            blackboard.SetVariableValue("AttackDamage", enemyData.attackDamage);
            blackboard.SetVariableValue("PatrolWaitTime", enemyData.patrolWaitTime);
        }

        #endregion

        #region Update



        private void UpdateBlackboard()
        {
            var blackboard = tree.blackboard;
            tree.Tick();
        }

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
        
        #region Combat

        public override void TakeDamage(float damage, Vector3 hitDirection)
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

        public void PerformAttack()
        {
            _lastAttackTime = Time.time;
            tree.blackboard.SetVariableValue("CanAttack", false);
            // Implement actual attack logic here
        }

        public void GetStunned(float duration)
        {
            duration /= enemyData.stunResistance;
            _isStunned = true;
            _stunDuration = duration;
            tree.blackboard.SetVariableValue("IsStunned", true);

            if (stateMachine != null)
            {
                CancelAttack();
            }
        }

        public void CancelAttack()
        {
            stateMachine?.TransitionToState(EnemyStateMachine.EnemyState.Idle);
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Save/Load

        public EnemySaveData GetSaveData()
        {
            return new EnemySaveData
            {
                id = enemyId,
                position = transform.position,
                rotation = transform.rotation,
                health = currentHealth,
                isDisengaged = _isDisengaged,
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

    }


}