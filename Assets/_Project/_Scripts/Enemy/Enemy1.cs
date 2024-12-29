using System;
using System.Collections.Generic;
using _Project._Scripts.ScriptBases;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = System.Numerics.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace _Project._Scripts.EnemyDir
{
    public class Enemy1 : MonoBehaviour, ICustomUpdate, IDamageable
    {
        [System.Serializable]
        public class EnemySaveData
        {
            public string                 id;
            public Vector3                position;
            public UnityEngine.Quaternion rotation;
            public float                  health;
            public bool                   isDisengaged;
            public bool                   isInActiveCombat;
        }
        public EnemyDefaultData enemyData;
        
        #region Serialized Fields
        
        [Header("Enemy id")]
        [SerializeField] private string enemyId = Guid.NewGuid().ToString();
        public string EnemyId => enemyId;
        [Header("Patrol Settings")]
        [SerializeField]List<Transform> waypoints;
        [SerializeField]NavMeshAgent agent;
        
        [Header("StateMachine")]
        [SerializeField] private EnemyStateMachine stateMachine;
        [SerializeField] private FloatingText dialogue;
        [SerializeField] private BehaviourTreeOwner tree;
        
        [SerializeField] private Transform player;
        
        #endregion
        
        #region State Variables
        
        private float _currentHealth;
        private bool  _isStunned;
        private float _stunDuration;
        private bool  _isInActiveCombat;
        private bool  _isDisengaged = true;
        private float _lastAttackTime;
        
        #endregion
        
        #region Properties
        
        public string Id            => enemyId;
        public float  CurrentHealth => _currentHealth;
        public bool   CanAttack     => Time.time - _lastAttackTime >= enemyData.attackCooldown;
        
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

        public void CustomUpdate()
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
            
            _currentHealth      = enemyData.maxHealth;
            agent.speed        = enemyData.moveSpeed;
            agent.angularSpeed = enemyData.rotationSpeed;
        }
        
        private void SetupBlackboard()
        {
            var blackboard = tree.blackboard;
            blackboard.SetVariableValue("Health", _currentHealth);
            blackboard.SetVariableValue("MaxHealth", enemyData.maxHealth);
            blackboard.SetVariableValue("IsStunned", _isStunned);
            blackboard.SetVariableValue("IsInCombat", _isInActiveCombat);
            blackboard.SetVariableValue("IsDisengaged", _isDisengaged);
            blackboard.SetVariableValue("PlayerTransform", player);
            blackboard.SetVariableValue("Waypoints", waypoints);
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
            blackboard.SetVariableValue("Health", _currentHealth);
            blackboard.SetVariableValue("IsStunned", _isStunned);
            blackboard.SetVariableValue("IsInCombat", _isInActiveCombat);
            blackboard.SetVariableValue("IsDisengaged", _isDisengaged);
            blackboard.SetVariableValue("CanAttack", CanAttack);

            if (player != null)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                blackboard.SetVariableValue("DistanceToPlayer", distanceToPlayer);
            }
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
        
        #region Combat
        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            float modifiedDamage = damage / enemyData.stunResistance;
            _currentHealth = Mathf.Max(0, _currentHealth - modifiedDamage);
            tree.blackboard.SetVariableValue("Health", _currentHealth);
            
            if (dialogue != null)
            {
                dialogue.ShowDialogue("Ouch!");
                dialogue.HideDialogue();
            }

            if (_currentHealth <= 0)
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
            duration     /= enemyData.stunResistance;
            _isStunned    =  true;
            _stunDuration =  duration;
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
                id               = enemyId,
                position         = transform.position,
                rotation         = transform.rotation,
                health           = _currentHealth,
                isDisengaged     = _isDisengaged,
                isInActiveCombat = _isInActiveCombat
            };
        }

        public void LoadSaveData(EnemySaveData data)
        {
            transform.position = data.position;
            transform.rotation = data.rotation;
            _currentHealth      = data.health;
            _isDisengaged       = data.isDisengaged;
            _isInActiveCombat   = data.isInActiveCombat;

            if (_currentHealth <= 0)
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