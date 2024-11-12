using System;
using UnityEngine;

public class NPCStateManager : StateManager<NPCStateManager.NPCState>
{
    public NPCState currentNPCState;
    public DialogueUI dialogueUI;
    public YarnDialogueManager yarnDialogueManager;
    public enum NPCState
    {
        Idle,
        Walking,
        Dialogue,
        Interacting
    }

    private void Awake()
    {
        States[NPCState.Idle] = new NPCIdle(NPCState.Idle, this);
        States[NPCState.Dialogue] = new NPCDialogue(NPCState.Dialogue,yarnDialogueManager, this);
        CurrentState = States[NPCState.Idle];
    }
    
    private void FixedUpdate()
    {
        NPCState nextStateKey = CurrentState.GetNextState();
        
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