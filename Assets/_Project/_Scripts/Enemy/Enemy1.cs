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
        #region Serialized Fields


        [Header("Patrol Settings")] [SerializeField]
        List<GameObject> waypoints;

        //[SerializeField]  protected NavMeshAgent  agent;

        [Header("StateMachine")] [SerializeField]
        private EnemyStateMachine stateMachine;
        

        
        
        
        #endregion

        #region State Variables




        #endregion

        #region Properties


        //used in the behaviour tree
        /*
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
        */
        //public override bool WantsAggressive { get; }


        //used in attack logic
        public bool CanAttack => Time.time - _lastAttackTime >= enemyData.attackCooldown;
        
        //public bool ShouldDisengage => DistanceToPlayer >= enemyData.disengageRange;
        



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

        #region Engagement


        
        

        #endregion
        
        #region Combat



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



        #endregion

    }


}