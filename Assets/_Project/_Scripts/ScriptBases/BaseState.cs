using UnityEngine;
using System;

public abstract class BaseState<EState> where EState : Enum
{
    protected BaseState(EState key)
    {
        StateKey = key;
    }
    
    public EState StateKey { get; private set; }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract EState GetNextState();
    public virtual void OnTriggerEnter(Collider other){}
    public virtual void OnTriggerStay(Collider other){}
    public virtual void OnTriggerExit(Collider other){}

}
