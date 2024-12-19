using _Project._Scripts.ScriptBases;
using UnityEngine;
using UnityEngine.AI;

namespace _Project._Scripts.EnemyDir.StateMachine
{
    public class EnemyAttackState : BaseState<EnemyStateMachine.EnemyState>
    {
        private readonly Transform playerTransform;
        private readonly Transform enemyTransform;
        private readonly float     attackRange    = 5f;
        private readonly float     attackCooldown = 4.0f;
        private          float     lastAttackTime;
        private          LayerMask playerLayer;
        
        private float damage = 10;
        
        public EnemyAttackState(EnemyStateMachine.EnemyState key, Transform playerTransform, 
                                Transform                    enemyTransform, LayerMask playerLayer) : base(key)
        {
            this.playerTransform = playerTransform;
            this.enemyTransform  = enemyTransform;
            this.playerLayer     = playerLayer;
        }

        public override void EnterState()
        {
            Debug.Log("Entering attack state");
            lastAttackTime = Time.time - attackCooldown; // Allow immediate first attack
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            if (playerTransform != null && Time.time >= lastAttackTime + attackCooldown)
            {
                StartAttack();
                lastAttackTime = Time.time;
            }
        }
        private void StartAttack()
        {
            // Play attack animation
            AttackPlayer();
        }

        private void AttackPlayer()
        {
            Collider[] hitColliders = Physics.OverlapSphere(enemyTransform.position, attackRange, playerLayer);
            foreach (Collider hit in hitColliders)
            {
                if(!IsLineOfSight(hit)) continue;
                IDamageable target = hit.GetComponent<IDamageable>();
                Vector3 hitDirection = (hit.transform.position - enemyTransform.position).normalized;
                target.TakeDamage(damage, hitDirection);    
                Debug.Log("Player hit for " + damage + " damage.");
            }
        }

        private bool IsLineOfSight(Collider target)
        {
            Vector3 direction = target.transform.position - enemyTransform.transform.position;
            if(Physics.Raycast(enemyTransform.transform.position, direction, out RaycastHit hit, attackRange))
            {
                return hit.collider == target;
            }
            return false;
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