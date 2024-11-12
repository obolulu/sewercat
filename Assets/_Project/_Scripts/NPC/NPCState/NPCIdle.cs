using UnityEngine;

public class NPCIdle : BaseState<NPCStateManager.NPCState>
{
    private NPCStateManager _stateManager;
    private NPCStateManager.NPCState _stateKey = NPCStateManager.NPCState.Idle;
    
    public NPCIdle(NPCStateManager.NPCState key,NPCStateManager stateManager) : base(key)
    {
        _stateManager = stateManager;
    }
    
    public override void EnterState()
    {
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }

    public override NPCStateManager.NPCState GetNextState()
    {
        return _stateKey;
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

    //public override NPCStateManager.NPCState StateKey => _stateKey;
}