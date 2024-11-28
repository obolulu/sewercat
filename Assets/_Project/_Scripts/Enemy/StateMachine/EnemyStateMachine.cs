using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Enemy.StateMachine;
using UnityEngine;

public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    public EnemyState currentEnemyState;
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
}