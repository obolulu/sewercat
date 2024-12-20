using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.EnemyDir.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using Animancer;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    public EnemyState currentEnemyState;
    private List<EnemyStateMachine.EnemyState> _priorityStatesList;
    
    [SerializeField] private Transform    _playerTransform;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private LayerMask   _playerLayer;
    
    [Header("Animations")]
    [SerializeField] private AnimancerComponent animancer;
    [SerializeField] private AnimationClip      attackClip;
    public enum EnemyState
    {
        Chase,
        Attack,
        Idle
    }
    private void Awake()
    {
        States[EnemyState.Attack] = new EnemyAttackState(EnemyState.Attack, _playerTransform, transform, _playerLayer,animancer, attackClip);
        States[EnemyState.Chase]  = new EnemyChaseState(EnemyState.Chase, _playerTransform, _navMeshAgent);
        States[EnemyState.Idle]  = new EnemyIdleState(EnemyState.Idle, _navMeshAgent, _playerTransform, transform);
        CurrentState = States[EnemyState.Chase];
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 5);
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