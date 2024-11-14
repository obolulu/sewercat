using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;
    protected bool IsTransitioningState;

    void Start()
    {
        CurrentState.EnterState();
    }
    void FixedUpdate()
    {
        // EState nextStateKey = CurrentState.GetNextState();
        // if (!IsTransitioningState&&nextStateKey.Equals(CurrentState.StateKey))
        // {
        //     CurrentState.UpdateState();
        // }
        // else if (!IsTransitioningState)
        // {
        //     TransitionToState(nextStateKey);
        // }
    }

    public void TransitionToState(EState stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    // not sure if "protected" is the right thing for the functions, check later
    protected void OnTriggerEnter(Collider other)
    {
        //CurrentState.OnTriggerEnter(other);
    }

    protected void OnTriggerStay(Collider other)
    {
        //throw new NotImplementedException();
    }

    protected void OnTriggerExit(Collider other)
    {
        //throw new NotImplementedException();
    }
    
}
