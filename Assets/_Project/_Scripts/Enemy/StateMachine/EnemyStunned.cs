using UnityEngine;

namespace _Project._Scripts.Enemy.StateMachine
{
    public class EnemyStunned : BaseState<EnemyStateMachine.EnemyState>
    {
        private EnemyStateMachine _enemyStateMachine;
        public EnemyStunned(EnemyStateMachine.EnemyState key, EnemyStateMachine enemyStateMachine) : base(key)
        {
            _enemyStateMachine = enemyStateMachine;
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override EnemyStateMachine.EnemyState GetNextState()
        {
            throw new System.NotImplementedException();
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