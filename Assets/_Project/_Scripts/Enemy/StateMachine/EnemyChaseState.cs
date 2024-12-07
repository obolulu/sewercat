using UnityEngine;
using UnityEngine.AI;

namespace _Project._Scripts.EnemyDir.StateMachine
{
    public class EnemyChaseState : BaseState<EnemyStateMachine.EnemyState>
    {
        private readonly Transform    playerTransform;
        private readonly NavMeshAgent navMeshAgent;
        private readonly float        attackRange = 4f; // Match the attack range with other states

        public EnemyChaseState(EnemyStateMachine.EnemyState key, Transform playerTransform, 
                               NavMeshAgent                 navMeshAgent) : base(key)
        {
            this.playerTransform = playerTransform;
            this.navMeshAgent    = navMeshAgent;
        }

        public override void EnterState()
        {
            navMeshAgent.isStopped = false;
        }

        public override void ExitState()
        {
            navMeshAgent.isStopped = true;
        }

        public override void UpdateState()
        {
            if (playerTransform != null)
            {
                navMeshAgent.SetDestination(playerTransform.position);
            }
        }

        public override EnemyStateMachine.EnemyState GetNextState()
        {
            if (playerTransform == null) return StateKey;

            float distanceToPlayer = Vector3.Distance(navMeshAgent.transform.position, playerTransform.position);
        
            // If close enough, transition to attack state
            if (distanceToPlayer <= attackRange)
            {
                return EnemyStateMachine.EnemyState.Attack;
            }
        
            // Otherwise keep chasing
            return EnemyStateMachine.EnemyState.Chase;
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