using System;
using System.Collections.Generic;
using _Project._Scripts.Enemy.BehaviourTree.Structure;
using _Project._Scripts.ScriptBases;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] List<Transform> _waypoints;
        private          bool            _isDamaged;
        NavMeshAgent                     _agent;
        //Animator _animator;
        BehaviourTree _tree;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _tree = new BehaviourTree("Enemy");
            var checkDamage    = new Leaf("Check Damage", new Condition(() => _isDamaged));
            var handleDamage   = new Leaf("Handle Damage", new ActionStrategy(HandleDamage));
            var damageSequence = new Sequence("Damage Sequence", 20);
            damageSequence.AddChild(checkDamage);
            damageSequence.AddChild(handleDamage);
            
            var patrol = new Leaf("Patrol", new PatrolStrategy(transform, _agent, _waypoints), 10);
            
            var prioritySelector = new PrioritySelector("Priority Selector");
            prioritySelector.AddChild(damageSequence);
            prioritySelector.AddChild(patrol);
            
            _tree.AddChild(prioritySelector);
        }

        private void Update()
        {
            //_animator.SetSpeed(_agent.velocity.magnitude);
            _tree.Evaluate();
        }

        public void TakeDamage(float damage, Vector3 hitDirection)
        {
            _isDamaged = true;
        }
        private void HandleDamage()
        {
            Debug.Log("Handling damage");
            _isDamaged = false; 
        }
    }