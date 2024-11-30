using UnityEngine;
using UnityEngine.AI;

namespace _Project._Scripts.Enemy.StateMachine
{
    public class EnemyAttackState : BaseState<EnemyStateMachine.EnemyState>
    {
        private readonly Transform    _playerTransform;
        private readonly Transform    _enemyTransform;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float _attackRange = 2.0f;
        private readonly float _attackCooldown = 1.0f;
        private float _lastAttackTime;

        public EnemyAttackState(EnemyStateMachine.EnemyState key, NavMeshAgent navMeshAgent, Transform playerTransform, Transform enemyTransform) : base(key)
        {
            _navMeshAgent = navMeshAgent;
            _playerTransform = playerTransform;
            _enemyTransform  = enemyTransform;
        }

        public override void EnterState()
        {
            _navMeshAgent.isStopped = true;
            _lastAttackTime = Time.time;
        }

        public override void ExitState()
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.Stop();
        }

        public override void UpdateState()
        {
            if (_playerTransform != null && Time.time >= _lastAttackTime + _attackCooldown)
            {
                AttackPlayer();
                _lastAttackTime = Time.time;
            }
        }

        private void AttackPlayer()
        {
            // Implement the logic to attack the player
            Debug.Log("Attacking the player");
        }

        public override EnemyStateMachine.EnemyState GetNextState()
        {
            if (_playerTransform != null && Vector3.Distance
                    (_enemyTransform.position, _playerTransform.position) > _attackRange)
            {
                return EnemyStateMachine.EnemyState.Chase;
            }
            return EnemyStateMachine.EnemyState.Attack;
        }

        public override void OnTriggerEnter(Collider other)
        {
            // Implement logic for when the enemy collides with something
        }

        public override void OnTriggerStay(Collider other)
        {
            // Implement logic for when the enemy stays in collision with something
        }

        public override void OnTriggerExit(Collider other)
        {
            // Implement logic for when the enemy exits collision with something
        }
    }
}