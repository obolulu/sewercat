using UnityEngine;
using UnityEngine.AI;

namespace _Project._Scripts.EnemyDir.StateMachine
{
    public class EnemyAttackState : BaseState<EnemyStateMachine.EnemyState>
    {
        private readonly Transform playerTransform;
        private readonly Transform enemyTransform;
        private readonly float     attackRange    = 4f;
        private readonly float     attackCooldown = 2.0f;
        private          float     lastAttackTime;

        public EnemyAttackState(EnemyStateMachine.EnemyState key, Transform playerTransform, 
                                Transform                    enemyTransform) : base(key)
        {
            this.playerTransform = playerTransform;
            this.enemyTransform  = enemyTransform;
        }

        public override void EnterState()
        {
            Debug.Log("Entering attack state");
            lastAttackTime = Time.time - attackCooldown; // Allow immediate first attack
        }

        public override void ExitState()
        {
            Debug.Log("Exiting attack state");
        }

        public override void UpdateState()
        {
            if (playerTransform != null && Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime += Time.time;
            }
        }

        private void AttackPlayer()
        {
            Debug.Log("Attacking the player");
            // Implement actual attack logic here
        }

        public override EnemyStateMachine.EnemyState GetNextState()
        {
            if (playerTransform == null) return StateKey;

            float distanceToPlayer = Vector3.Distance(enemyTransform.position, playerTransform.position);
        
            // If too far, chase
            if (distanceToPlayer > attackRange)
            {
                return EnemyStateMachine.EnemyState.Chase;
            }
        
            // If attack is on cooldown, go to idle
            if (Time.time < lastAttackTime + attackCooldown)
            {
                return EnemyStateMachine.EnemyState.Idle;
            }
        
            // Otherwise stay in attack state
            return EnemyStateMachine.EnemyState.Attack;
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