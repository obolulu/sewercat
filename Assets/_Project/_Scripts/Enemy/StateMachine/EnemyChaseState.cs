using UnityEngine;
using UnityEngine.AI;

namespace _Project._Scripts.EnemyDir.StateMachine
{
    public class EnemyChaseState : BaseState<EnemyStateMachine.EnemyState>
    {
        private readonly Transform    _playerTransform;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly float        _attackRange = 5.0f;

        public EnemyChaseState(EnemyStateMachine.EnemyState key, Transform playerTransform, NavMeshAgent navMeshAgent) : base(key)
        {
            _playerTransform = playerTransform;
            _navMeshAgent    = navMeshAgent;
        }

        public override void EnterState()
        {
            _navMeshAgent.isStopped = false;
        }

        public override void ExitState()
        {
            _navMeshAgent.isStopped = true;
        }

        public override void UpdateState()
        {
            if (_playerTransform != null)
            {
                _navMeshAgent.SetDestination(_playerTransform.position);
            }        
        }

        public override EnemyStateMachine.EnemyState GetNextState()
        {
            if (_playerTransform != null && Vector3.Distance
                    (_navMeshAgent.transform.position, _playerTransform.position) <= _attackRange)
            {
                return EnemyStateMachine.EnemyState.Attack;
            }
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