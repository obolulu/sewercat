using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Enemy.StateMachine;
using UnityEngine;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    public EnemyState currentEnemyState;
    private Stack<BaseState<EnemyState>> _stateStack = new Stack<BaseState<EnemyState>>(); // for priority states
    private List<EnemyStateMachine.EnemyState> _priorityStatesList;

    public enum EnemyState
    {
        Idle,
        Stunned,
        Dying
    }
    private void Awake()
    {
        States[EnemyState.Idle]    = new EnemyIdleState(EnemyState.Idle, this);
        States[EnemyState.Stunned] = new EnemyDying(EnemyState.Stunned, this);
        States[EnemyState.Dying]   = new EnemyStunned(EnemyState.Dying, this);
        CurrentState               = States[EnemyState.Idle];
    }

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

    void CheckPriorityStates()
    {
        if (_stateStack.Count > 0 //&& !IsInPriorityState()
                                  )
        {
            PopPriorityState();
        }
    }
    private void PopPriorityState()
    {
        if (_stateStack.Count > 0)
        {
            CurrentState.ExitState();
            CurrentState = _stateStack.Pop();
            CurrentState.EnterState();
        }
    }
}