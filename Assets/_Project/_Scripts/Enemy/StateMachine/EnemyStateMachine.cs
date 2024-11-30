using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Enemy.StateMachine;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    public EnemyState currentEnemyState;
    private List<EnemyStateMachine.EnemyState> _priorityStatesList;
    
    [SerializeField] private Transform    _playerTransform;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    public enum EnemyState
    {
        Chase,
        Attack
    }
    private void Awake()
    {
        States[EnemyState.Attack] = new EnemyAttackState(EnemyState.Attack,_navMeshAgent, _playerTransform, transform);
        States[EnemyState.Chase]  = new EnemyChaseState(EnemyState.Chase, _playerTransform, _navMeshAgent);
        CurrentState = States[EnemyState.Chase];
    }
    
    /*
    void FixedUpdate()
    {
        EnemyState nextStateKey = CurrentState.GetNextState();
        CheckPriorityStates();
        
        if (!IsTransitioningState&&nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }
    */
    public void CustomUpdate()
    {
        EnemyState nextStateKey = CurrentState.GetNextState();
        if (!IsTransitioningState&&nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }
}