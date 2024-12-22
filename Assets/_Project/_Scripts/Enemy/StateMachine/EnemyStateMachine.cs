    using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.EnemyDir.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using Animancer;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
public class EnemyStateMachine : StateManager<EnemyStateMachine.EnemyState>
{
    public EnemyState currentEnemyState;
    private List<EnemyStateMachine.EnemyState> _priorityStatesList;
    
    [SerializeField] private Transform    _playerTransform;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private LayerMask   _playerLayer;
    
    
    [FoldoutGroup("Animations")]
    [SerializeField] private AnimancerComponent animancer;
    [FoldoutGroup("Animations")]
    [BoxGroup("Animations/Attack Animation")]
    [SerializeField] private AnimationClip attackClip;
    public AnimationClip AttackClip => attackClip;
    [FoldoutGroup("Animations")]
    [BoxGroup("Animations/Attack Animation")]
    [SerializeField] private MMFeedbacks attackFeedbacks;
    public MMFeedbacks AttackFeedbacks => attackFeedbacks;

    public enum EnemyState
    {
        Chase,
        Attack,
        Idle
    }
    private void Awake()
    {
        States[EnemyState.Attack] = new EnemyAttackState(EnemyState.Attack, _playerTransform, transform, _playerLayer,animancer, attackClip,this);
        States[EnemyState.Chase]  = new EnemyChaseState(EnemyState.Chase, _playerTransform, _navMeshAgent);
        States[EnemyState.Idle]  = new EnemyIdleState(EnemyState.Idle, _navMeshAgent, _playerTransform, transform);
        CurrentState = States[EnemyState.Chase];
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 5);
    }

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
}