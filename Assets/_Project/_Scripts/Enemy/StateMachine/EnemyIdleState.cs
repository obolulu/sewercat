using UnityEngine;
using UnityEngine.AI;

namespace _Project._Scripts.EnemyDir.StateMachine
{
    public class EnemyIdleState : BaseState<EnemyStateMachine.EnemyState>
    {
        private readonly Transform    playerTransform;
        private readonly Transform    enemyTransform;
        private readonly NavMeshAgent navMeshAgent;
        private readonly float        attackRange    = 4f;
        private readonly float        attackCooldown = 2.0f;
        private          float        lastAttackTime;
        private readonly float        chaseRange = 15.0f; // Added chase range

        public EnemyIdleState(EnemyStateMachine.EnemyState key,             NavMeshAgent navMeshAgent, 
                              Transform                    playerTransform, Transform    enemyTransform) : base(key)
        {
            this.navMeshAgent    = navMeshAgent;
            this.playerTransform = playerTransform;
            this.enemyTransform  = enemyTransform;
        }

        public override void EnterState()
        {
            UnityEngine.Debug.Log("entering idle state");
            navMeshAgent.isStopped = true;
            lastAttackTime         = Time.time;
        }

        public override void ExitState()
        {
            navMeshAgent.isStopped = false;
        }

        public override void UpdateState()
        {
            // Just wait during the cooldown
            // No need to set destination while idle
        }

        public override EnemyStateMachine.EnemyState GetNextState()
        {
            if (playerTransform == null) return StateKey;

            float distanceToPlayer = Vector3.Distance(enemyTransform.position, playerTransform.position);
        
            // If player is too far, chase them
            if (distanceToPlayer > attackRange && distanceToPlayer <= chaseRange)
            {
                return EnemyStateMachine.EnemyState.Chase;
            }
        
            // If attack cooldown is over and player is in range, attack
            if (Time.time >= lastAttackTime + attackCooldown && distanceToPlayer <= attackRange)
            {
                return EnemyStateMachine.EnemyState.Attack;
            }
        
            // Otherwise stay idle
            return EnemyStateMachine.EnemyState.Idle;
        }

        public override void OnTriggerEnter(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerStay(Collider other)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerExit(Collider other)
        {
            throw new System.NotImplementedException();
        }
    }
}