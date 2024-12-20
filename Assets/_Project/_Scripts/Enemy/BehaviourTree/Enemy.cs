using System;
using System.Collections.Generic;
using _Project._Scripts.EnemyDir;
using _Project._Scripts.EnemyDir.BehaviourTree.Structure;
using _Project._Scripts.EnemyDir;
using _Project._Scripts.ScriptBases;
using UnityEngine;
using UnityEngine.AI;
// ReSharper disable ComplexConditionExpression


public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("Enemy id")]
        [SerializeField] private string enemyId = Guid.NewGuid().ToString();
        public string EnemyId => enemyId;
        
        [Header("Patrol Settings")]
        [SerializeField]List<Transform> waypoints;
        [SerializeField]NavMeshAgent agent;

        [Header("Combat Settings")] 
        [SerializeField] private float disengageRange = 20f;
        [SerializeField] private float     engageRange = 10f;
        [SerializeField] private float     maxHealth   = 100f;
        [SerializeField] private Transform player;
        
        [Header("StateMachine")]
        [SerializeField] private EnemyStateMachine enemyStateMachine;
        [SerializeField] private FloatingText dialogue;
        

        BehaviourTree                    _tree;
        private float _currentHealth;
        private          bool            _isDamaged;
        private bool _isInActiveCombat = true;
        private bool _isDisengaged = true;
        public EnemySaveData enemySaveData;
        private bool isStunned = false;
        private float stunTimer = 0f;
        
    
    #region Save/Load
    public EnemySaveData GetSaveData()
    {
        enemySaveData = new EnemySaveData
        {
            id = enemyId,
            position = transform.position,
            rotation = transform.rotation,
            health = _currentHealth,
            isDisengaged = _isDisengaged,
            isInActiveCombat = _isInActiveCombat,
        };
        return enemySaveData;    
    }

    public void LoadSaveData(EnemySaveData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;
        _currentHealth = data.health;
        _isDisengaged = data.isDisengaged;
        _isInActiveCombat = data.isInActiveCombat;
        //Debug.Log("health:" + _currentHealth);
        if(_currentHealth <= 0)
        {
            HandleDeath();
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    #endregion
    
    #region setup

    private void SetupBehaviourTree()
    
{
    _tree = new BehaviourTree("Enemy");

    // Check low health -> die sequence (top priority)
    var checkLowHealth = new Leaf("Check Low Health", new Condition(() => _currentHealth <= 0));
    var handleDying = new Leaf("Handle Dying", new ActionStrategy(HandleDying));
    var dyingSequence = new Sequence("Dying Sequence", 40);
    dyingSequence.AddChild(checkLowHealth);
    dyingSequence.AddChild(handleDying);

    // Combat Logic: Engaged State
    var checkPlayerWithinDisengageRange = new Leaf("Check Player Within Disengage Range",
        new Condition(() => _isInActiveCombat && 
                            Vector3.Distance(transform.position, player.position) < disengageRange));
    var handleCombat = new Leaf("Handle Combat", new ActionStrategy(() =>
    {
        //Debug.Log("Handling combat...");
        HandleCombat();
    }));
    var combatSequence = new Sequence("Combat Sequence", 30);
    combatSequence.AddChild(checkPlayerWithinDisengageRange);
    combatSequence.AddChild(handleCombat);

    // Disengage Logic: Reset to Patrol
    var checkPlayerOutOfDisengageRange = new Leaf("Check Player Out Of Disengage Range",
        new Condition(() => _isInActiveCombat && 
                            Vector3.Distance(transform.position, player.position) >= disengageRange));
    var handleDisengage = new Leaf("Handle Disengage", new ActionStrategy(() =>
    {
        Debug.Log("Disengaging from combat, resetting to patrol...");
        _isInActiveCombat = false;
        _isDisengaged = true;
    }));
    var disengageSequence = new Sequence("Disengage Sequence", 20);
    disengageSequence.AddChild(checkPlayerOutOfDisengageRange);
    disengageSequence.AddChild(handleDisengage);

    // Engage Logic: Disengaged State
    var checkPlayerWithinEngageRange = new Leaf("Check Player Within Engage Range",
        new Condition(() => !_isInActiveCombat && 
                            Vector3.Distance(transform.position, player.position) <= engageRange));
    var handleEngageCombat = new Leaf("Handle Engage Combat", new ActionStrategy(() =>
    {
        Debug.Log("Engaging in combat...");
        _isInActiveCombat = true;
        _isDisengaged = false;
    }));
    var engageSequence = new Sequence("Engage Sequence", 20);
    engageSequence.AddChild(checkPlayerWithinEngageRange);
    engageSequence.AddChild(handleEngageCombat);

    // Patrol Logic: Only when disengaged
    var checkNotInCombat = new Leaf("Check Not In Combat", new Condition(() => !_isInActiveCombat));
    var patrol = new Leaf("Patrol", new PatrolStrategy(transform, agent, waypoints));
    var patrolSequence = new Sequence("Patrol Sequence", 10);
    patrolSequence.AddChild(checkNotInCombat);
    patrolSequence.AddChild(patrol);

    // Main Behavior Selector
    var prioritySelector = new PrioritySelector("Priority Selector");
    prioritySelector.AddChild(dyingSequence);
    prioritySelector.AddChild(combatSequence);
    prioritySelector.AddChild(disengageSequence);
    prioritySelector.AddChild(engageSequence);
    prioritySelector.AddChild(patrolSequence);

    _tree.AddChild(prioritySelector);
}
    private void Awake()
    
        {
            if (string.IsNullOrEmpty(enemyId))
            {
                enemyId = System.Guid.NewGuid().ToString();
            }
            
            _currentHealth = maxHealth;
            SetupBehaviourTree();
        }

    #endregion
        
    #region stun
    public void GetStunned(float duration)
    {
        isStunned = true;
        stunTimer = duration;
    }
    
    public void CancelAttack()
    {
        // Reset attack state
        enemyStateMachine.TransitionToState(EnemyStateMachine.EnemyState.Idle);
    }
    #endregion
        private void Update()
        {
            //_animator.SetSpeed(_agent.velocity.magnitude);
            _tree.Evaluate();
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            _isDamaged = true;
            _currentHealth -= damage;
            dialogue.ShowDialogue("Ouch!");
            dialogue.HideDialogue();
        }

        private void HandleDying()
        {
            Debug.Log("Handling death");
            HandleDeath();
        }
        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }
        private void HandleAttack()
        {
            
        }

        private void HandleCombat()
        {
            enemyStateMachine.CustomUpdate();
        }
        private void HandleDamage()
        {
            Debug.Log("Handling damage");
            _isDamaged = false; 
        }
    }