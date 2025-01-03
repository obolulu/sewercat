using System;
using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.EnemyDir;
using _Project._Scripts.ScriptBases;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace _Project._Scripts.Enemy
{
    public class Enemy1 : EnemyBase, ICustomUpdate, IDamageable
    {
        #region Serialized Fields

        [Header("Patrol Settings")] 
        [SerializeField] private bool isEnemyPatrolling = false;
        [ShowIf("isEnemyPatrolling")]
        [SerializeField] List<GameObject> waypoints;
        
        [Header("StateMachine")] [SerializeField]
        private EnemyStateMachine stateMachine;
        
        
        #endregion

        #region State Variables




        #endregion

        #region Properties


        //used in attack logic
        //public bool CanAttack => Time.time - _lastAttackTime >= enemyData.attackCooldown;

        //is used in the combat manager to determine if the enemy should engage
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







        
        /*
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
        */

    }


}
